using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArackiralamaProje.Models
{
    [Table("Cars")]  // Veritabanı tablo adını belirtme
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka seçimi zorunludur.")]
        [Display(Name = "Marka")]
        public int CarBrandId { get; set; }

        [ForeignKey("CarBrandId")]
        [Display(Name = "Marka Bilgisi")]
        public virtual CarBrand CarBrand { get; set; }

        [Required(ErrorMessage = "Model adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Model adı 2-100 karakter arasında olmalıdır.")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Üretim yılı zorunludur.")]
        [Range(1900, 2100, ErrorMessage = "Geçerli bir üretim yılı giriniz (1900-2100).")]
        [Display(Name = "Üretim Yılı")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Yakıt tipi seçimi zorunludur.")]
        [Display(Name = "Yakıt Tipi")]
        public int FuelTypeId { get; set; }

        [ForeignKey("FuelTypeId")]
        [Display(Name = "Yakıt Tipi Bilgisi")]
        public virtual FuelType FuelType { get; set; }

        [Required(ErrorMessage = "Vites tipi seçimi zorunludur.")]
        [Display(Name = "Vites Tipi")]
        public int GearTypeId { get; set; }

        [ForeignKey("GearTypeId")]
        [Display(Name = "Vites Tipi Bilgisi")]
        public virtual GearType GearType { get; set; }

        [Required(ErrorMessage = "Plaka numarası zorunludur.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Plaka numarası 5-20 karakter arasında olmalıdır.")]
        [Display(Name = "Plaka Numarası")]
        [RegularExpression(@"^[0-9]{2}-[A-Z]{1,3}-[0-9]{2,4}$", ErrorMessage = "Geçerli bir plaka formatı giriniz (Örnek: 34-ABC-123)")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Günlük kira ücreti zorunludur.")]
        [Range(0.01, 10000, ErrorMessage = "Günlük kira ücreti 0.01 ile 10.000 arasında olmalıdır.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Günlük Kira Ücreti")]
        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL adresi giriniz.")]
        [Display(Name = "Fotoğraf URL")]
        [StringLength(500, ErrorMessage = "URL en fazla 500 karakter olabilir.")]
        public string ImageUrl { get; set; }

        [Display(Name = "Müsaitlik Durumu")]
        public bool IsAvailable { get; set; } = true;

        // Navigation property
        [Display(Name = "Kiralama Geçmişi")]
        public virtual ICollection<Rental> Rentals { get; set; } = new HashSet<Rental>();

        // Ek metadata
        [Display(Name = "Oluşturulma Tarihi")]
        [ScaffoldColumn(false)]  // Formlarda gösterilmez
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncellenme")]
        [ScaffoldColumn(false)]
        public DateTime? LastUpdated { get; set; }
    }
}