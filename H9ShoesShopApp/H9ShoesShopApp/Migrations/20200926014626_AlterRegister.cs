using Microsoft.EntityFrameworkCore.Migrations;

namespace H9ShoesShopApp.Migrations
{
    public partial class AlterRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductID",
                table: "OrderDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductID",
                table: "OrderDetails");
        }
    }
}
