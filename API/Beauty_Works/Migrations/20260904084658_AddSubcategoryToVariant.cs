using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Works.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcategoryToVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubcategoryID",
                table: "Variants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Variants_SubcategoryID",
                table: "Variants",
                column: "SubcategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Subcategories_SubcategoryID",
                table: "Variants",
                column: "SubcategoryID",
                principalTable: "Subcategories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Subcategories_SubcategoryID",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_SubcategoryID",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "SubcategoryID",
                table: "Variants");
        }
    }
}
