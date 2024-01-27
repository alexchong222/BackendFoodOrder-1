using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendFoodOrder.Migrations
{
    public partial class database3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "taskName",
                table: "Product",
                newName: "productName");

            migrationBuilder.RenameColumn(
                name: "taskDesc",
                table: "Product",
                newName: "productDesc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "productName",
                table: "Product",
                newName: "taskName");

            migrationBuilder.RenameColumn(
                name: "productDesc",
                table: "Product",
                newName: "taskDesc");
        }
    }
}
