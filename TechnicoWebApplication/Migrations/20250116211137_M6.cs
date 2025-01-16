using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class M6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Repairs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PropertyOwners",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_Date",
                table: "Repairs",
                column: "RepairDate");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_Status",
                table: "Repairs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyOwner_Email",
                table: "PropertyOwners",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Repair_Date",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repair_Status",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_PropertyOwner_Email",
                table: "PropertyOwners");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PropertyOwners",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
