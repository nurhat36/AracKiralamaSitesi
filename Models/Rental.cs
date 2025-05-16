namespace ArackiralamaProje.Models
{
    public class Rental
    {
        public int Id { get; set; }                     // Kiralama ID

        public int CustomerId { get; set; }             // Kiralayan müşteri ID'si
        public Customer Customer { get; set; }          // Müşteri ilişkisi

        public int CarId { get; set; }                  // Kiralanan araç ID'si
        public Car Car { get; set; }                    // Araç ilişkisi

        public DateTime RentDate { get; set; }          // Kiralama başlangıç tarihi
        public DateTime ReturnDate { get; set; }        // Araç teslim tarihi

        public decimal TotalPrice { get; set; }         // Toplam kira ücreti
        public bool IsReturned { get; set; }            // Araç teslim edildi mi?

        public Payment Payment { get; set; }            // Bu kiralama için yapılan ödeme
    }

}
