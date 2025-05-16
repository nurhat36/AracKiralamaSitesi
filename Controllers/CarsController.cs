using ArackiralamaProje.Data;
using ArackiralamaProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArackiralamaProje.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public CarsController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("CarBrandId,FuelTypeId,GearTypeId,Model,Year,PlateNumber,PricePerDay,ImageUrl,IsAvailable")] Car car)
        {
            Console.WriteLine($"Gelen veri - CarBrandId: {car.CarBrandId}, FuelTypeId: {car.FuelTypeId}, GearTypeId: {car.GearTypeId}");
            try
            {
                

                // Yeni araç oluştur
                var newCar = new Car
                {
                    CarBrandId = car.CarBrandId,
                    FuelTypeId = car.FuelTypeId,
                    GearTypeId = car.GearTypeId,
                    Model = car.Model,
                    Year = car.Year,
                    PlateNumber = car.PlateNumber,
                    PricePerDay = car.PricePerDay,
                    ImageUrl = car.ImageUrl,
                    IsAvailable = car.IsAvailable,
                    CreatedDate = DateTime.Now
                };

                _context.Cars.Add(newCar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                await PopulateDropdowns(car);
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                return View(car);
            }
        }

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
