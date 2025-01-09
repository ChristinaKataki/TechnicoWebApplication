using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicoWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyItemId",
                table: "Repairs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_PropertyItemId",
                table: "Repairs",
                column: "PropertyItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_PropertyItems_PropertyItemId",
                table: "Repairs",
                column: "PropertyItemId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_PropertyItems_PropertyItemId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_PropertyItemId",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "PropertyItemId",
                table: "Repairs");
        }
    }
}
