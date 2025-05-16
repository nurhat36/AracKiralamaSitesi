namespace ArackiralamaProje.Models
{
    public class Car
    {
        public int Id { get; set; }                     // Araç ID (birincil anahtar)

        public int CarBrandId { get; set; }             // Araç markasının ID'si (Yabancı anahtar)
        public CarBrand CarBrand { get; set; }          // Marka bilgisine erişim (ilişki)

        public string Model { get; set; }               // Araç modeli (örnek: A4, Focus)
        public int Year { get; set; }                   // Üretim yılı

        public int FuelTypeId { get; set; }             // Yakıt tipi ID'si
        public FuelType FuelType { get; set; }          // Yakıt tipi ilişkisi

        public int GearTypeId { get; set; }             // Vites tipi ID'si
        public GearType GearType { get; set; }          // Vites tipi ilişkisi

        public string PlateNumber { get; set; }         // Plaka numarası
        public decimal PricePerDay { get; set; }        // Günlük kira ücreti

        public string ImageUrl { get; set; }            // Aracın fotoğrafının URL adresi
        public bool IsAvailable { get; set; }           // Araç kiralanabilir mi? (true = müsait)

        public ICollection<Rental> Rentals { get; set; } // Bu araçla yapılan kiralamalar
    }

}
