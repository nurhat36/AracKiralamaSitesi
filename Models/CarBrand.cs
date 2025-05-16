namespace ArackiralamaProje.Models
{
    public class CarBrand
    {
        public int Id { get; set; }                     // Marka tablosunun birincil anahtarı (örneğin: 1 = BMW)
        public string Name { get; set; }                // Marka adı (örneğin: BMW, Audi)

        public ICollection<Car> Cars { get; set; }      // Bu markaya ait araçlar (ilişkili araçlar listesi)
    }

}
