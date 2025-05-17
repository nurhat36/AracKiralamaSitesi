using ArackiralamaProje.Data;
using ArackiralamaProje.Models;
using ArackiralamaProje.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Authorize]
public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<CustomerController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı.");
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            // Customer yoksa yeni bir model oluştur
            if (customer == null)
            {
                customer = new Customer
                {
                    UserId = user.Id,
                    FullName = user.Email?.Split('@')[0] ?? "Kullanıcı",
                    PhoneNumber = user.PhoneNumber ?? string.Empty
                };
            }

            return View(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Profil sayfası yüklenirken hata oluştu");
            TempData["ErrorMessage"] = "Profil bilgileri yüklenirken bir hata oluştu";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(Customer model)
    {
        try
        {
           

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Telefon numarası validasyonu
            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                Console.WriteLine("Telefon numarası zorunludur");
                ModelState.AddModelError("PhoneNumber", "Telefon numarası zorunludur");
                return View(model);
            }

            // Customer var mı kontrol et
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (existingCustomer == null)
            {
                Console.WriteLine("Profil bilgileriniz başarıyla oluşturuldu");
                // Yeni müşteri oluştur
                model.UserId = user.Id;
                _context.Customers.Add(model);
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla oluşturuldu";
            }
            else
            {
                // Mevcut müşteriyi güncelle
                existingCustomer.FullName = model.FullName;
                existingCustomer.PhoneNumber = model.PhoneNumber;
                existingCustomer.DrivingLicenseNumber = model.DrivingLicenseNumber;
                existingCustomer.Address = model.Address;
                existingCustomer.BirthDate = model.BirthDate;
                existingCustomer.SecondaryEmail = model.SecondaryEmail;
                existingCustomer.UpdatedAt = DateTime.UtcNow;

                _context.Customers.Update(existingCustomer);
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi";
                Console.WriteLine("Profil bilgileriniz başarıyla güncellendi");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CompleteProfile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Profil güncellenirken hata oluştu");
            TempData["ErrorMessage"] = "Profil güncellenirken bir hata oluştu: " + ex.Message;
            return View(model);
        }
    }
    [HttpGet]
    public async Task<IActionResult> CompleteProfile()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (customer == null)
            {
                return View(new CustomerProfileViewModel());
            }

            // TempData'dan mesajları oku (varsa)
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            // Modeli doldur
            var model = new CustomerProfileViewModel
            {
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                DrivingLicenseNumber = customer.DrivingLicenseNumber,
                Address = customer.Address
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CompleteProfile sayfası yüklenirken hata oluştu");
            TempData["ErrorMessage"] = "Sayfa yüklenirken bir hata oluştu";
            return RedirectToAction("Profile");
        }
    }


    // CompleteProfile action'larını kaldırabilirsiniz çünkü artık Profile action'ı her iki işlevi de görüyor
}