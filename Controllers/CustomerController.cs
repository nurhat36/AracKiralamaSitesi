using ArackiralamaProje.Data;
using ArackiralamaProje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArackiralamaProje.Controllers
{
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
                    _logger.LogWarning("Kullanıcı bulunamadı. User: {User}", User);
                    return RedirectToAction("Login", "Account");
                }

                var customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (customer == null)
                {
                    // Sadece yeni customer oluştur, CompleteProfile'e yönlendirme yapma
                    customer = new Customer
                    {
                        UserId = user.Id,
                        FullName = user.Email?.Split('@')[0] ?? "Kullanıcı",
                        PhoneNumber = user.PhoneNumber ?? string.Empty,
                        DrivingLicenseNumber = string.Empty,
                        Address = string.Empty
                    };

                    // Customer'ı veritabanına eklemeden view'e gönder
                    return View(customer);
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
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Telefon numarası validasyonu
                if (string.IsNullOrEmpty(model.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Telefon numarası zorunludur");
                    return View(model);
                }

                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (existingCustomer == null)
                {
                    // Yeni müşteri oluştur
                    model.UserId = user.Id;
                    _context.Customers.Add(model);

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla kaydedildi";
                    return RedirectToAction("Profile");
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

                    _context.Customers.Update(existingCustomer);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi";
                    return RedirectToAction("Profile");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil güncellenirken hata oluştu");
                TempData["ErrorMessage"] = "Profil güncellenirken bir hata oluştu";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CompleteProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(CustomerProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var customer = new Customer
                {
                    UserId = user.Id,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    DrivingLicenseNumber = model.DrivingLicenseNumber,
                    Address = model.Address
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla kaydedildi";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil tamamlama sırasında hata");
                TempData["ErrorMessage"] = "Profil bilgileri kaydedilirken bir hata oluştu";
                return View(model);
            }
        }
    }

    public class CustomerProfileViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Ehliyet numarası en fazla 50 karakter olabilir")]
        public string DrivingLicenseNumber { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        public string Address { get; set; }
    }
}