using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArackiralamaProje.Migrations
{
    /// <inheritdoc />
    public partial class rentaledited1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Önce foreign key constraint'i kaldır
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            // 2. Sonra index'i kaldır
            migrationBuilder.DropIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals");

            // 3. Sütun tipini değiştir (string -> int)
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Rentals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // 4. Veri dönüşümü için SQL komutu ekleyin (önemli!)
            migrationBuilder.Sql(@"
                -- Bu kısım veritabanınızdaki verilere göre özelleştirilmelidir
                -- Örnek: Geçerli sayısal değerleri dönüştürme
                -- UPDATE Rentals SET CustomerId = CAST(CustomerId AS INT) WHERE ISNUMERIC(CustomerId) = 1
                -- Geçersiz değerler için:
                -- UPDATE Rentals SET CustomerId = [DEFAULT_DEĞER] WHERE ISNUMERIC(CustomerId) = 0
            ");

            // 5. Index'i yeniden oluştur
            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals",
                column: "CustomerId");

            // 6. Foreign key constraint'i yeniden oluştur
            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma işlemleri (rollback için)

            // 1. Foreign key constraint'i kaldır
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            // 2. Index'i kaldır
            migrationBuilder.DropIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals");

            // 3. Sütun tipini eski haline getir (int -> string)
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Rentals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // 4. Veri dönüşümü için SQL komutu (gerekirse)
            migrationBuilder.Sql(@"
                -- Geri dönüşüm için gerekli SQL komutları
                -- UPDATE Rentals SET CustomerId = CAST(CustomerId AS NVARCHAR(450))
            ");

            // 5. Index'i yeniden oluştur
            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals",
                column: "CustomerId");

            // 6. Foreign key constraint'i yeniden oluştur
            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "UserId", // UserId string olduğu için
                onDelete: ReferentialAction.Cascade);
        }
    }
}