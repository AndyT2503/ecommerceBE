using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Domain.Migrations
{
    public partial class Update_Comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                schema: "product",
                table: "Comment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                schema: "product",
                table: "ChildComment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_IsAdmin",
                schema: "product",
                table: "Comment",
                column: "IsAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_ChildComment_IsAdmin",
                schema: "product",
                table: "ChildComment",
                column: "IsAdmin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_IsAdmin",
                schema: "product",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_ChildComment_IsAdmin",
                schema: "product",
                table: "ChildComment");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                schema: "product",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                schema: "product",
                table: "ChildComment");
        }
    }
}
