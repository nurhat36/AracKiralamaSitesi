using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ArackiralamaProje.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Ehliyet numarası en fazla 50 karakter olabilir")]
        public string DrivingLicenseNumber { get; set; }

        public ICollection<Rental> Rentals { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Adres")]
        public string Address { get; set; } = string.Empty; // Varsayılan değer

        [EmailAddress]
        [Display(Name = "Alternatif Email")]
        public string SecondaryEmail { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Son Güncelleme")]
        public DateTime? UpdatedAt { get; set; }
    }
}