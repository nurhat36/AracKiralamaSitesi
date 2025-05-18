using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ArackiralamaProje.Models;

namespace ArackiralamaProje.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<GearType> GearTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Comment>()
                .HasOne(c => c.Car)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.CarId);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            // GearType seed verisi
            builder.Entity<GearType>().HasData(
                new GearType { Id = 1, Name = "Manuel" },
                new GearType { Id = 2, Name = "Otomatik" },
                new GearType { Id = 3, Name = "Yarı Otomatik" }
            );

            // FuelType seed verisi
            builder.Entity<FuelType>().HasData(
                new FuelType { Id = 1, Name = "Benzin" },
                new FuelType { Id = 2, Name = "Dizel" },
                new FuelType { Id = 3, Name = "Elektrik" },
                new FuelType { Id = 4, Name = "Hibrit" },
                new FuelType { Id = 5, Name = "LPG" }
            );

            // CarBrand seed verisi
            builder.Entity<CarBrand>().HasData(
                new CarBrand { Id = 1, Name = "BMW" },
                new CarBrand { Id = 2, Name = "Audi" },
                new CarBrand { Id = 3, Name = "Mercedes" },
                new CarBrand { Id = 4, Name = "Volkswagen" },
                new CarBrand { Id = 5, Name = "Toyota" },
                new CarBrand { Id = 6, Name = "Ford" },
                new CarBrand { Id = 7, Name = "Honda" },
                new CarBrand { Id = 8, Name = "Nissan" },
                new CarBrand { Id = 9, Name = "Hyundai" },
                new CarBrand { Id = 10, Name = "Kia" },
                new CarBrand { Id = 11, Name = "Renault" },
                new CarBrand { Id = 12, Name = "Peugeot" },
                new CarBrand { Id = 13, Name = "Chevrolet" },
                new CarBrand { Id = 14, Name = "Mazda" },
                new CarBrand { Id = 15, Name = "Subaru" },
                new CarBrand { Id = 16, Name = "Tesla" },
                new CarBrand { Id = 17, Name = "Jaguar" },
                new CarBrand { Id = 18, Name = "Volvo" },
                new CarBrand { Id = 19, Name = "Land Rover" },
                new CarBrand { Id = 20, Name = "Mini" }
            );
            builder.Entity<Rental>(entity =>
            {
                // Trigger tanımı
                entity.ToTable(tb => tb.HasTrigger("TR_Rental_UpdateCarAvailability"));
            });

            // Customer <-> ApplicationUser ilişkisi (1-1)
            builder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Rental <-> Customer ilişkisi (N-1)
            builder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rental <-> Payment ilişkisi (1-1)
            builder.Entity<Rental>()
                .HasOne(r => r.Payment)
                .WithOne(p => p.Rental)
                .HasForeignKey<Payment>(p => p.RentalId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Customer>()
                .HasIndex(c => c.UserId)
                .IsUnique();

        }
    }
}
