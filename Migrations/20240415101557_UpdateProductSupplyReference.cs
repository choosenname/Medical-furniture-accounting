using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalFurnitureAccounting.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSupplyReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Supplies_SupplyId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SupplyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Products",
                newName: "SuppplyId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                newName: "IX_Products_SuppplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Supplies_SuppplyId",
                table: "Products",
                column: "SuppplyId",
                principalTable: "Supplies",
                principalColumn: "SupplyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Supplies_SuppplyId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SuppplyId",
                table: "Products",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SuppplyId",
                table: "Products",
                newName: "IX_Products_SupplierId");

            migrationBuilder.AddColumn<int>(
                name: "SupplyId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplyId",
                table: "Products",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Supplies_SupplyId",
                table: "Products",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "SupplyId");
        }
    }
}
