using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class M5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_PropertyOwners_PropertyOwnerVat",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_PropertyOwnerVat",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "PropertyOwnerVat",
                table: "Repairs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyOwnerVat",
                table: "Repairs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_PropertyOwnerVat",
                table: "Repairs",
                column: "PropertyOwnerVat");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_PropertyOwners_PropertyOwnerVat",
                table: "Repairs",
                column: "PropertyOwnerVat",
                principalTable: "PropertyOwners",
                principalColumn: "Vat");
        }
    }
}
