using ArackiralamaProje.Data;
using ArackiralamaProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArackiralamaProje.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Araç listeleme
        public async Task<IActionResult> Index()
        {
            try
            {
                var cars = await _context.Cars
                    .Include(c => c.CarBrand)
                    .Include(c => c.FuelType)
                    .Include(c => c.GearType)
                    .Include(c => c.Images) // Resimleri de dahil ediyoruz
                    .OrderBy(c => c.CarBrand.Name)
                    .ThenBy(c => c.Model)
                    .AsNoTracking()
                    .ToListAsync();

                return View(cars);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata (Index): " + ex.Message);
                return View("Error");
            }
        }

        // Araç detayları
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.CarBrand)
                .Include(c => c.FuelType)
                .Include(c => c.GearType)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            // Müsaitlik kontrolü
            ViewBag.IsCarAvailable = !await _context.Rentals
                .AnyAsync(r => r.CarId == id &&
                              r.ReturnDate >= DateTime.Today &&
                              !r.IsReturned);

            return View("~/Views/Cars/Details.cshtml", car);
        }

        // Araç ekleme sayfası (GET)
        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateDropdowns();
                return View(new Car { IsAvailable = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata (Create GET): " + ex.Message);
                return View("Error");
            }
        }

        // Araç ekleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarBrandId,FuelTypeId,GearTypeId,Model,Year,PlateNumber,PricePerDay,ImageUrl,IsAvailable")] Car car, List<IFormFile> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    car.CreatedDate = DateTime.Now;
                    _context.Add(car);
                    await _context.SaveChangesAsync();

                    // Resimleri kaydet
                    if (files != null && files.Count > 0)
                    {
                        await SaveCarImages(car.Id, files);
                    }

                    return RedirectToAction(nameof(Index));
                }
                await PopulateDropdowns(car);
                return View(car);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata (Create POST): " + ex.Message);
                await PopulateDropdowns(car);
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                return View(car);
            }
        }

        // Resim ekleme sayfası
        public async Task<IActionResult> AddImages(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.CarId = carId;
            return View();
        }

        // Resim ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> AddImages(int carId, List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    ModelState.AddModelError("", "Lütfen en az bir resim seçin");
                    ViewBag.CarId = carId;
                    return View();
                }

                await SaveCarImages(carId, files);
                return RedirectToAction(nameof(Details), new { id = carId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata (AddImages): " + ex.Message);
                ModelState.AddModelError("", "Resim yükleme sırasında hata: " + ex.Message);
                ViewBag.CarId = carId;
                return View();
            }
        }

        // Ana resim belirleme
        [HttpPost]
        public async Task<IActionResult> SetMainImage(int carId, int imageId)
        {
            try
            {
                // Tüm resimlerin ana resim özelliğini false yap
                var images = await _context.CarImages
                    .Where(x => x.CarId == carId)
                    .ToListAsync();

                foreach (var img in images)
                {
                    img.IsMainImage = false;
                }

                // Seçilen resmi ana resim yap
                var mainImage = images.FirstOrDefault(x => x.Id == imageId);
                if (mainImage != null)
                {
                    mainImage.IsMainImage = true;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = carId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata (SetMainImage): " + ex.Message);
                return RedirectToAction(nameof(Details), new { id = carId });
            }
        }

        // Resim silme
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int carId, int imageId)
        {
            try
            {
                var image = await _context.CarImages.FindAsync(imageId);
                if (image != null)
                {
                    // Fiziksel dosyayı sil
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    _context.CarImages.Remove(image);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Details), new { id = carId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata (DeleteImage): " + ex.Message);
                return RedirectToAction(nameof(Details), new { id = carId });
            }
        }

        // Yardımcı metot: Resimleri kaydet
        private async Task SaveCarImages(int carId, List<IFormFile> files)
        {
            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var image = new CarImage
                    {
                        ImageUrl = "/uploads/" + uniqueFileName,
                        CarId = carId,
                        IsMainImage = false
                    };

                    _context.CarImages.Add(image);
                }
            }

            await _context.SaveChangesAsync();
        }

        // Dropdown listelerini doldurma
        private async Task PopulateDropdowns(Car car = null)
        {
            ViewBag.CarBrands = new SelectList(
                await _context.CarBrands.OrderBy(x => x.Name).AsNoTracking().ToListAsync(),
                "Id", "Name", car?.CarBrandId);

            ViewBag.FuelTypes = new SelectList(
                await _context.FuelTypes.OrderBy(x => x.Name).AsNoTracking().ToListAsync(),
                "Id", "Name", car?.FuelTypeId);

            ViewBag.GearTypes = new SelectList(
                await _context.GearTypes.OrderBy(x => x.Name).AsNoTracking().ToListAsync(),
                "Id", "Name", car?.GearTypeId);
        }
    }
}