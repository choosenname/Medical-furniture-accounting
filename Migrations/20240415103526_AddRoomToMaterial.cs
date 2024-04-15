using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalFurnitureAccounting.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomToMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Materials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Room",
                table: "Materials");
        }
    }
}
