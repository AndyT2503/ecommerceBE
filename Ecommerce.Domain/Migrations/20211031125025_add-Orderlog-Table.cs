using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Domain.Migrations
{
    public partial class addOrderlogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderLog",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLog_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "order",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_CreatedAt",
                schema: "order",
                table: "OrderLog",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_OrderId",
                schema: "order",
                table: "OrderLog",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_Status",
                schema: "order",
                table: "OrderLog",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLog",
                schema: "order");
        }
    }
}
