using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Works.Migrations
{
    /// <inheritdoc />
    public partial class RedoImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Subcategories_Types_ProductTypeID",
            //    table: "Subcategories");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Types_Categories_CategoryID",
            //    table: "Types");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Types",
            //    table: "Types");

            //migrationBuilder.RenameTable(
            //    name: "Types",
            //    newName: "ProductTypes");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Types_CategoryID",
            //    table: "ProductTypes",
            //    newName: "IX_ProductTypes_CategoryID");

            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "Products",
                type: "int",
                nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ProductTypes",
            //    table: "ProductTypes",
            //    column: "ID");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImageID",
                table: "Products",
                column: "ImageID",
                unique: true,
                filter: "[ImageID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Image_ImageID",
                table: "Products",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductTypes_Categories_CategoryID",
            //    table: "ProductTypes",
            //    column: "CategoryID",
            //    principalTable: "Categories",
            //    principalColumn: "ID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Subcategories_ProductTypes_ProductTypeID",
            //    table: "Subcategories",
            //    column: "ProductTypeID",
            //    principalTable: "ProductTypes",
            //    principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Image_ImageID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_Categories_CategoryID",
                table: "ProductTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_ProductTypes_ProductTypeID",
                table: "Subcategories");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImageID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypes",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ProductTypes",
                newName: "Types");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTypes_CategoryID",
                table: "Types",
                newName: "IX_Types_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Types_ProductTypeID",
                table: "Subcategories",
                column: "ProductTypeID",
                principalTable: "Types",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Types_Categories_CategoryID",
                table: "Types",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID");
        }
    }
}
