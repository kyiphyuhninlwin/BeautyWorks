using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Works.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subcategories_SubcategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Variants_VariantID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SubcategoryId",
                table: "Products",
                newName: "SubcategoryID");

            migrationBuilder.RenameColumn(
                name: "VariantID",
                table: "Products",
                newName: "StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SubcategoryId",
                table: "Products",
                newName: "IX_Products_SubcategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_VariantID",
                table: "Products",
                newName: "IX_Products_StatusID");

            migrationBuilder.AddColumn<int>(
                name: "TypeID",
                table: "Subcategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    ProductsID = table.Column<int>(type: "int", nullable: false),
                    VariantsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => new { x.ProductsID, x.VariantsID });
                    table.ForeignKey(
                        name: "FK_ProductVariant_Products_ProductsID",
                        column: x => x.ProductsID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Variants_VariantsID",
                        column: x => x.VariantsID,
                        principalTable: "Variants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_TypeID",
                table: "Subcategories",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_VariantsID",
                table: "ProductVariant",
                column: "VariantsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products",
                column: "BrandID",
                principalTable: "Brands",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Status_StatusID",
                table: "Products",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subcategories_SubcategoryID",
                table: "Products",
                column: "SubcategoryID",
                principalTable: "Subcategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Types_TypeID",
                table: "Subcategories",
                column: "TypeID",
                principalTable: "Types",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Status_StatusID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subcategories_SubcategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Types_TypeID",
                table: "Subcategories");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_Subcategories_TypeID",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TypeID",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SubcategoryID",
                table: "Products",
                newName: "SubcategoryId");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "Products",
                newName: "VariantID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SubcategoryID",
                table: "Products",
                newName: "IX_Products_SubcategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StatusID",
                table: "Products",
                newName: "IX_Products_VariantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subcategories_SubcategoryId",
                table: "Products",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Variants_VariantID",
                table: "Products",
                column: "VariantID",
                principalTable: "Variants",
                principalColumn: "ID");
        }
    }
}
