namespace ArackiralamaProje.Models
{
    public class FuelType
    {
        public int Id { get; set; }                     // Yakıt türü ID (örneğin: 1 = Benzin)
        public string Name { get; set; }                // Yakıt türü adı (Benzin, Dizel, Elektrik)

        public ICollection<Car> Cars { get; set; }      // Bu yakıt türüne sahip araçlar
    }

}
