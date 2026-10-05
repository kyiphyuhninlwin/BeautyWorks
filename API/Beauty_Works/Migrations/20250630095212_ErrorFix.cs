using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Works.Migrations
{
    /// <inheritdoc />
    public partial class ErrorFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Types_TypeID",
                table: "Subcategories");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subcategories",
                newName: "ID");

            //migrationBuilder.RenameColumn(
            //    name: "TypeID",
            //    table: "Subcategories",
            //    newName: "ProductTypeID");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Subcategories_TypeID",
            //    table: "Subcategories",
            //    newName: "IX_Subcategories_ProductTypeID");

            migrationBuilder.AddColumn<bool>(
                name: "HasVariant",
                table: "Subcategories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Types_ProductTypeID",
                table: "Subcategories",
                column: "ProductTypeID",
                principalTable: "ProductTypes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_ProductTypes_ProductTypeID",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "HasVariant",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Subcategories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProductTypeID",
                table: "Subcategories",
                newName: "TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Subcategories_ProductTypeID",
                table: "Subcategories",
                newName: "IX_Subcategories_ProductTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_ProductTypes_ProductTypeID",
                table: "Subcategories",
                column: "TypeID",
                principalTable: "ProductTypes",
                principalColumn: "ID");
        }
    }
}
