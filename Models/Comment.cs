using System.ComponentModel.DataAnnotations;

namespace ArackiralamaProje.Models
{
    // Models/Comment.cs
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 yıldız

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // İlişkiler
        public int CarId { get; set; }
        public Car Car { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
