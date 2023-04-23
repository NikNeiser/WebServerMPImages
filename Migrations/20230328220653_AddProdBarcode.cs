using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebServerMPImages.Migrations
{
    /// <inheritdoc />
    public partial class AddProdBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Barcode",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");
        }
    }
}
