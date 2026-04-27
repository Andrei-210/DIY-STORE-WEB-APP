using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIY_STORE.Migrations
{
    public partial class AddCategoryImageAndShopAvailability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Image column to Categories
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Create ProductShopAvailabilities table
            migrationBuilder.CreateTable(
                name: "ProductShopAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    InStock = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShopAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductShopAvailabilities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductShopAvailabilities_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductShopAvailabilities_ProductId",
                table: "ProductShopAvailabilities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductShopAvailabilities_ShopId",
                table: "ProductShopAvailabilities",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ProductShopAvailabilities");
            migrationBuilder.DropColumn(name: "Image", table: "Categories");
        }
    }
}
