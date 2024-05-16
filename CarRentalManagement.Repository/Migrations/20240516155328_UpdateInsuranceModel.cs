using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalManagement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInsuranceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Vehicles_VehicleId",
                table: "Insurances");

            // Alter the VehicleId column to be nullable
            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Insurances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Add the foreign key constraint back with the 'Restrict' delete behavior
            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Vehicles_VehicleId",
                table: "Insurances",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the modified foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Vehicles_VehicleId",
                table: "Insurances");

            // Alter the VehicleId column to be non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Insurances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Add the original foreign key constraint back with the 'Cascade' delete behavior
            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Vehicles_VehicleId",
                table: "Insurances",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
