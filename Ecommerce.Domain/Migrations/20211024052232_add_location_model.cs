using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Domain.Migrations
{
    public partial class add_location_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "location");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "location",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                schema: "location",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Province_Country_CountryCode",
                        column: x => x.CountryCode,
                        principalSchema: "location",
                        principalTable: "Country",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "District",
                schema: "location",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProvinceCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Code);
                    table.ForeignKey(
                        name: "FK_District_Province_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalSchema: "location",
                        principalTable: "Province",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ward",
                schema: "location",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DistrictCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ward", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Ward_District_DistrictCode",
                        column: x => x.DistrictCode,
                        principalSchema: "location",
                        principalTable: "District",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                schema: "location",
                table: "Country",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_District_Name",
                schema: "location",
                table: "District",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_District_ProvinceCode",
                schema: "location",
                table: "District",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Province_CountryCode",
                schema: "location",
                table: "Province",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Province_Name",
                schema: "location",
                table: "Province",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Ward_DistrictCode",
                schema: "location",
                table: "Ward",
                column: "DistrictCode");

            migrationBuilder.CreateIndex(
                name: "IX_Ward_Name",
                schema: "location",
                table: "Ward",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_District_DistrictCode",
                schema: "order",
                table: "Order",
                column: "DistrictCode",
                principalSchema: "location",
                principalTable: "District",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Province_ProvinceCode",
                schema: "order",
                table: "Order",
                column: "ProvinceCode",
                principalSchema: "location",
                principalTable: "Province",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_District_DistrictCode",
                schema: "order",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Province_ProvinceCode",
                schema: "order",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Ward",
                schema: "location");

            migrationBuilder.DropTable(
                name: "District",
                schema: "location");

            migrationBuilder.DropTable(
                name: "Province",
                schema: "location");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "location");
        }
    }
}
