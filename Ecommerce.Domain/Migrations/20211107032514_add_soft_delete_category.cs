using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Domain.Migrations
{
    public partial class add_soft_delete_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderLog_CreatedAt",
                schema: "order",
                table: "OrderLog");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "product",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "product",
                table: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_CreatedAt",
                schema: "order",
                table: "OrderLog",
                column: "CreatedAt");
        }
    }
}
