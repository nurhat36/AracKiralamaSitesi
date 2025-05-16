using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArackiralamaProje.Models
{
    public class Customer
    {
        public int Id { get; set; }                          // Müşteri ID (int olarak kalabilir)

        [ForeignKey("User")]                                 // ForeignKey attribute ekledik
        public string UserId { get; set; }                   // AspNetUsers tablosundaki ID (string)

        public ApplicationUser User { get; set; }            // IdentityUser yerine ApplicationUser

        public string FullName { get; set; }                 // Ad Soyad
        public string PhoneNumber { get; set; }              // Telefon
        public string DrivingLicenseNumber { get; set; }     // Ehliyet No

        public ICollection<Rental> Rentals { get; set; }     // Kiralamalar
    }
}