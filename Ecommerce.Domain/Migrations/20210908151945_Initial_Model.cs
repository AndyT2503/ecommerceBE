using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Domain.Migrations
{
    public partial class Initial_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "product");

            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "ProductType",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleCode",
                schema: "order",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Percent = table.Column<int>(type: "integer", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCode", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderCode = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    ProvinceCode = table.Column<string>(type: "text", nullable: true),
                    DistrictCode = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CustomerName = table.Column<string>(type: "text", nullable: true),
                    SaleCode = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true),
                    PaymentStatus = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValueSql: "'Waiting'::text"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_SaleCode_SaleCode",
                        column: x => x.SaleCode,
                        principalSchema: "order",
                        principalTable: "SaleCode",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    AvailableStatus = table.Column<string>(type: "text", nullable: true),
                    Configuration = table.Column<object>(type: "jsonb", nullable: true, defaultValueSql: "'{ }'::jsonb"),
                    SpecialFeatures = table.Column<string>(type: "text", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalSchema: "product",
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Product_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "product",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProductType",
                schema: "product",
                columns: table => new
                {
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProductType", x => new { x.SupplierId, x.ProductTypeId });
                    table.ForeignKey(
                        name: "FK_SupplierProductType_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalSchema: "product",
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierProductType_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "product",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "product",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "order",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildComment",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildComment_Comment_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "product",
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Image",
                schema: "product",
                table: "Category",
                column: "Image");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                schema: "product",
                table: "Category",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Price",
                schema: "product",
                table: "Category",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ProductId",
                schema: "product",
                table: "Category",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildComment_CommentId",
                schema: "product",
                table: "ChildComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildComment_Content",
                schema: "product",
                table: "ChildComment",
                column: "Content");

            migrationBuilder.CreateIndex(
                name: "IX_ChildComment_Username",
                schema: "product",
                table: "ChildComment",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Content",
                schema: "product",
                table: "Comment",
                column: "Content");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProductId",
                schema: "product",
                table: "Comment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Username",
                schema: "product",
                table: "Comment",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Address",
                schema: "order",
                table: "Order",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DistrictCode",
                schema: "order",
                table: "Order",
                column: "DistrictCode");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderCode",
                schema: "order",
                table: "Order",
                column: "OrderCode");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentMethod",
                schema: "order",
                table: "Order",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentStatus",
                schema: "order",
                table: "Order",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProvinceCode",
                schema: "order",
                table: "Order",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SaleCode",
                schema: "order",
                table: "Order",
                column: "SaleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status",
                schema: "order",
                table: "Order",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CategoryId",
                schema: "order",
                table: "OrderDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                schema: "order",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Price",
                schema: "order",
                table: "OrderDetail",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Quantity",
                schema: "order",
                table: "OrderDetail",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "IX_Product_AvailableStatus",
                schema: "product",
                table: "Product",
                column: "AvailableStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name",
                schema: "product",
                table: "Product",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Product_OriginalPrice",
                schema: "product",
                table: "Product",
                column: "OriginalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductTypeId",
                schema: "product",
                table: "Product",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Slug",
                schema: "product",
                table: "Product",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SpecialFeatures",
                schema: "product",
                table: "Product",
                column: "SpecialFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Status",
                schema: "product",
                table: "Product",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierId",
                schema: "product",
                table: "Product",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductType_Code",
                schema: "product",
                table: "ProductType",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductType_Name",
                schema: "product",
                table: "ProductType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_Comment",
                schema: "product",
                table: "Rating",
                column: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ImageUrl",
                schema: "product",
                table: "Rating",
                column: "ImageUrl");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ProductId",
                schema: "product",
                table: "Rating",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_Rate",
                schema: "product",
                table: "Rating",
                column: "Rate");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCode_Code",
                schema: "order",
                table: "SaleCode",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCode_MaxPrice",
                schema: "order",
                table: "SaleCode",
                column: "MaxPrice");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCode_Percent",
                schema: "order",
                table: "SaleCode",
                column: "Percent");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCode_ValidUntil",
                schema: "order",
                table: "SaleCode",
                column: "ValidUntil");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Code",
                schema: "product",
                table: "Supplier",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Name",
                schema: "product",
                table: "Supplier",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProductType_ProductTypeId",
                schema: "product",
                table: "SupplierProductType",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProductType_SupplierId",
                schema: "product",
                table: "SupplierProductType",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_User_FirstName",
                schema: "auth",
                table: "User",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastName",
                schema: "auth",
                table: "User",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_User_Password",
                schema: "auth",
                table: "User",
                column: "Password");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role",
                schema: "auth",
                table: "User",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                schema: "auth",
                table: "User",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildComment",
                schema: "product");

            migrationBuilder.DropTable(
                name: "OrderDetail",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Rating",
                schema: "product");

            migrationBuilder.DropTable(
                name: "SupplierProductType",
                schema: "product");

            migrationBuilder.DropTable(
                name: "User",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "product");

            migrationBuilder.DropTable(
                name: "SaleCode",
                schema: "order");

            migrationBuilder.DropTable(
                name: "ProductType",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "product");
        }
    }
}
