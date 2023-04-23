using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebServerMPImages.Migrations
{
    /// <inheritdoc />
    public partial class DropProdBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhoto_Products_ProductBarcode",
                table: "ProductPhoto");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProductBarcode",
                table: "ProductPhoto",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPhoto_ProductBarcode",
                table: "ProductPhoto",
                newName: "IX_ProductPhoto_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhoto_Products_ProductId",
                table: "ProductPhoto",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhoto_Products_ProductId",
                table: "ProductPhoto");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Barcode");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductPhoto",
                newName: "ProductBarcode");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPhoto_ProductId",
                table: "ProductPhoto",
                newName: "IX_ProductPhoto_ProductBarcode");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhoto_Products_ProductBarcode",
                table: "ProductPhoto",
                column: "ProductBarcode",
                principalTable: "Products",
                principalColumn: "Barcode");
        }
    }
}
