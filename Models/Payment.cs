namespace ArackiralamaProje.Models
{
    public class Payment
    {
        public int Id { get; set; }                     // Ödeme ID
        public int RentalId { get; set; }               // Hangi kiralama için ödeme yapıldı?
        public Rental Rental { get; set; }              // Kiralama ilişkisi

        public DateTime PaymentDate { get; set; }       // Ödeme tarihi
        public string PaymentMethod { get; set; }       // Ödeme yöntemi (Kart, Havale vb.)
        public decimal Amount { get; set; }             // Ödeme tutarı
        public bool IsSuccessful { get; set; }          // Başarılı mı?
    }

}
