using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Domain.Migrations
{
    public partial class addusernamerating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "product",
                table: "Rating",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UserName",
                schema: "product",
                table: "Rating",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rating_UserName",
                schema: "product",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "product",
                table: "Rating");
        }
    }
}
