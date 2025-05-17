using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArackiralamaProje.Migrations
{
    /// <inheritdoc />
    public partial class rentaledited123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rentals_RentalId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            

            

            

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rentals_RentalId",
                table: "Payments",
                column: "RentalId",
                principalTable: "Rentals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rentals_RentalId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerId1",
                table: "Rentals",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rentals_RentalId",
                table: "Payments",
                column: "RentalId",
                principalTable: "Rentals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId1",
                table: "Rentals",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
