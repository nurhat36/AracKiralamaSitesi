using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArackiralamaProje.Migrations
{
    /// <inheritdoc />
    public partial class trigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TRIGGER TR_Rental_UpdateCarAvailability
            ON Rentals
            AFTER INSERT, UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;
                
                UPDATE c
                SET c.IsAvailable = 0
                FROM Cars c
                INNER JOIN inserted i ON c.Id = i.CarId
                WHERE i.IsReturned = 0 AND i.ReturnDate >= GETDATE();
                
                -- Süresi dolan araçları müsait yap
                UPDATE c
                SET c.IsAvailable = 1
                FROM Cars c
                INNER JOIN inserted i ON c.Id = i.CarId
                WHERE i.IsReturned = 1 OR i.ReturnDate < GETDATE();
            END
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER TR_Rental_UpdateCarAvailability");
        }
    }
}
