namespace ArackiralamaProje.Models
{
    public class GearType
    {
        public int Id { get; set; }                     // Vites tipi ID (örneğin: 1 = Otomatik)
        public string Name { get; set; }                // Vites tipi adı (Otomatik, Manuel)

        public ICollection<Car> Cars { get; set; }      // Bu vites tipine sahip araçlar
    }

}
