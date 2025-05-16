using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArackiralamaProje.Migrations
{
    /// <inheritdoc />
    public partial class ImproveRentalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualReturnDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Rentals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Rentals",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OdometerReadingAtRent",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OdometerReadingAtReturn",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Rentals",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualReturnDate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "OdometerReadingAtRent",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "OdometerReadingAtReturn",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Rentals");
        }
    }
}
