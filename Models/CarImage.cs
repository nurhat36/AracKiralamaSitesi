using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArackiralamaProje.Models
{
    public class CarImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool IsMainImage { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}