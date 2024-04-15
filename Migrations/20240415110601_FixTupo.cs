using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalFurnitureAccounting.Migrations
{
    /// <inheritdoc />
    public partial class FixTupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Supplies_SuppplyId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SuppplyId",
                table: "Products",
                newName: "SupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SuppplyId",
                table: "Products",
                newName: "IX_Products_SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Supplies_SupplyId",
                table: "Products",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "SupplyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Supplies_SupplyId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SupplyId",
                table: "Products",
                newName: "SuppplyId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplyId",
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
    }
}
