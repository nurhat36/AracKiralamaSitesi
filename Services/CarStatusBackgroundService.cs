using ArackiralamaProje.Data;
using Microsoft.EntityFrameworkCore;

namespace ArackiralamaProje.Services
{
    public class CarStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CarStatusBackgroundService> _logger;

        public CarStatusBackgroundService(IServiceProvider services, ILogger<CarStatusBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    try
                    {
                        // Süresi dolmuş kiralık araçları bul
                        var expiredRentals = await dbContext.Rentals
                            .Where(r => r.ReturnDate < DateTime.Now && !r.IsReturned)
                            .Include(r => r.Car)
                            .ToListAsync(stoppingToken);

                        foreach (var rental in expiredRentals)
                        {
                            rental.IsReturned = true;
                            rental.Car.IsAvailable = true;
                            _logger.LogInformation($"Araç {rental.CarId} müsait duruma getirildi");
                        }

                        if (expiredRentals.Any())
                        {
                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Araç durum güncelleme hatası");
                    }
                }

                // Her saat başı kontrol et
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
