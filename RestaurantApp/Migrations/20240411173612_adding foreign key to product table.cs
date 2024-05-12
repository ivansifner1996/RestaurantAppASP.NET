using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.Migrations
{
    public partial class addingforeignkeytoproducttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Product_MenuId",
                table: "Product",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Menu_MenuId",
                table: "Product",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Menu_MenuId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_MenuId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Product");
        }
    }
}
