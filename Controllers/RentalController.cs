using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArackiralamaProje.Data;
using ArackiralamaProje.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

[Authorize]
public class RentalController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RentalController> _logger;

    public RentalController(
     ApplicationDbContext context,
     UserManager<ApplicationUser> userManager,
     ILogger<RentalController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }
    // Kiralama formunu göster
    [HttpGet]
    public IActionResult Create(int carId)
    {
        Console.WriteLine("buraya girdi");
        var car = _context.Cars
            .Include(c => c.CarBrand)
            .FirstOrDefault(c => c.Id == carId);

        if (car == null) return NotFound();

        var model = new Rental
        {
            CarId = carId,
            RentDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(1)
        };

        ViewBag.CarInfo = $"{car.CarBrand.Name} {car.Model}";
        ViewBag.DailyPrice = car.PricePerDay;

        return View(model);
    }
    public async Task<IActionResult> MyRentals()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == currentUser.Id);

        if (customer == null)
        {
            return NotFound("Müşteri bilgisi bulunamadı.");
        }

        var rentals = await _context.Rentals
            .Include(r => r.Car)
                .ThenInclude(c => c.CarBrand)
            .Where(r => r.CustomerId == customer.Id)
            .OrderByDescending(r => r.RentDate)
            .ToListAsync();

        return View(rentals);
    }


    // Kiralama işlemini işle
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Rental rental)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                Console.WriteLine("Kiralama yapabilmek için giriş yapmalısınız");
                ModelState.AddModelError("", "Kiralama yapabilmek için giriş yapmalısınız");
                await PopulateViewData(rental.CarId);
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            Console.WriteLine("burayamı girdin");
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == currentUser.Id);

            var rentalDays = (rental.ReturnDate - rental.RentDate).Days;
            if (rentalDays < 1)
            {
                ModelState.AddModelError("", "Minimum kiralama süresi 1 gündür");
                await PopulateViewData(rental.CarId);
                return View(rental);
            }

            var car = await _context.Cars.FindAsync(rental.CarId);
            if (car == null)
            {
                ModelState.AddModelError("", "Araç bulunamadı");
                await PopulateViewData(rental.CarId);
                return View(rental);
            }

            // Müsaitlik kontrolü
            // Müsaitlik kontrolü (artık trigger da kontrol edecek)
            bool isCarAvailable = await _context.Cars
                .Where(c => c.Id == rental.CarId && c.IsAvailable)
                .AnyAsync();

            if (!isCarAvailable)
            {
                ModelState.AddModelError("", "Bu araç şu anda müsait değil");
                await PopulateViewData(rental.CarId);
                return View(rental);
            }

            rental.CustomerId = customer.Id;
            rental.TotalPrice = rentalDays * car.PricePerDay;
            rental.CreatedAt = DateTime.Now;
            rental.IsReturned = false; // Yeni eklenen alan

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync(); // Trigger burada tetiklenecek

            return RedirectToAction("Success", new { id = rental.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kiralama oluşturulurken hata");
            ModelState.AddModelError("", "İşlem sırasında bir hata oluştu");
            await PopulateViewData(rental.CarId);
            return View(rental);
        }
    }


    private async Task PopulateViewData(int carId)
    {
        var car = await _context.Cars
            .Include(c => c.CarBrand)
            .FirstOrDefaultAsync(c => c.Id == carId);

        if (car != null)
        {
            ViewBag.CarInfo = $"{car.CarBrand?.Name ?? "Marka Yok"} {car.Model}";
            ViewBag.DailyPrice = car.PricePerDay;
        }
    }

    // Başarılı kiralama sayfası
    public IActionResult Success(int id)
    {
        var rental = _context.Rentals
            .Include(r => r.Car)
            .ThenInclude(c => c.CarBrand)
            .FirstOrDefault(r => r.Id == id);

        return View(rental);
    }
}