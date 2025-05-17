using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArackiralamaProje.Migrations
{
    /// <inheritdoc />
    public partial class rentaledited12345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId1",
                table: "Customers",
                column: "UserId",
                unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
