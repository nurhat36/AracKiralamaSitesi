using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArackiralamaProje.Migrations
{
    /// <inheritdoc />
    public partial class MakeAddressNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                nullable: true, // NULL değerlere izin ver
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "", // Varsayılan değer
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
