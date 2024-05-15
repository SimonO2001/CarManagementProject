using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalManagement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class vehicle_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HorsePower",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Torque",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorsePower",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Torque",
                table: "Vehicles");
        }
    }
}
