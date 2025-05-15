using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ArackiralamaProje.Models; // ApplicationUser sınıfının bulunduğu namespace

namespace ArackiralamaProje.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Buraya ileride başka tablolar da ekleyebilirsin, örneğin:
        // public DbSet<Arac> Araclar { get; set; }
    }
}
