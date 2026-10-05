using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Works.Migrations
{
    /// <inheritdoc />
    public partial class ImageFileExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Image_ImageID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Images");

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Images_ImageID",
                table: "Products",
                column: "ImageID",
                principalTable: "Images",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Images_ImageID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Image_ImageID",
                table: "Products",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ID");
        }
    }
}
