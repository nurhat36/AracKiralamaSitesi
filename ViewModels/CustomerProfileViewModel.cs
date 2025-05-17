// ViewModels/CustomerProfileViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace ArackiralamaProje.ViewModels
{
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