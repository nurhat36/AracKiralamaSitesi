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
            var cars = await _context.Cars
                .Include(c => c.CarBrand)
                .Include(c => c.FuelType)
                .Include(c => c.GearType)
                .ToListAsync();
            return View(cars);
        }

        // Araç ekleme sayfası (GET)
        public IActionResult Create()
        {
            // ViewBag veya ViewData ile dropdownlar için veri gönder
            ViewBag.CarBrands = new SelectList(_context.CarBrands, "Id", "Name");
            ViewBag.FuelTypes = new SelectList(_context.FuelTypes, "Id", "Name");
            ViewBag.GearTypes = new SelectList(_context.GearTypes, "Id", "Name");


            return View();
        }

        // Araç ekleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            // ModelState hatalarını loglayın
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hataları görmek için
                }

                ViewBag.CarBrands = new SelectList(_context.CarBrands, "Id", "Name", car.CarBrandId);
                ViewBag.FuelTypes = new SelectList(_context.FuelTypes, "Id", "Name", car.FuelTypeId);
                ViewBag.GearTypes = new SelectList(_context.GearTypes, "Id", "Name", car.GearTypeId);

                return View(car);
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
