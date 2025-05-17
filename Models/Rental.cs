using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArackiralamaProje.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        public int CarId { get; set; }
        public Car Car { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RentDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [GreaterThan(nameof(RentDate), ErrorMessage = "Teslim tarihi kiralama tarihinden sonra olmalıdır")]
        public DateTime ReturnDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ActualReturnDate { get; set; }

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Approved, Active, Completed, Cancelled

        public bool IsReturned { get; set; }

        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public int? OdometerReadingAtRent { get; set; }
        public int? OdometerReadingAtReturn { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    // Özel validation attribute
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}