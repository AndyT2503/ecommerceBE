using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Ecommerce.Domain.Migrations
{
    public partial class addnotificationmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "product",
                table: "Comment",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Seen = table.Column<bool>(type: "boolean", nullable: false),
                    MetaData = table.Column<object>(type: "jsonb", nullable: true, defaultValueSql: "'{ }'::jsonb"),
                    EventName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedAt",
                schema: "order",
                table: "Order",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                schema: "product",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CreatedAt",
                schema: "notification",
                table: "Notification",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_EventName",
                schema: "notification",
                table: "Notification",
                column: "EventName");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_GroupName",
                schema: "notification",
                table: "Notification",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                schema: "notification",
                table: "Notification",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification",
                schema: "notification");

            migrationBuilder.DropIndex(
                name: "IX_Order_CreatedAt",
                schema: "order",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Comment_UserId",
                schema: "product",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "product",
                table: "Comment");
        }
    }
}
