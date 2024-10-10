using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.App.Migrations.ECommerceDb
{
    /// <inheritdoc />
    public partial class InitialECommerce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ECommerce");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seller",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingStatusEnum",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMovementTypeEnum",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovementTypeEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ECommerce",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Seller_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "ECommerce",
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerUser",
                schema: "ECommerce",
                columns: table => new
                {
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerUser", x => new { x.SellerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SellerUser_Seller_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "ECommerce",
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerUser_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderTrace",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrace_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "ECommerce",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "ECommerce",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipping",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShippedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeliveredDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    FinishedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipping_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "ECommerce",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shipping_ShippingStatusEnum_Status",
                        column: x => x.Status,
                        principalSchema: "ECommerce",
                        principalTable: "ShippingStatusEnum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryProduct",
                schema: "ECommerce",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProduct", x => new { x.CategoriesId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "ECommerce",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "ECommerce",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "ECommerce",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "ECommerce",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "ECommerce",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "ECommerce",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMovement",
                schema: "ECommerce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    MovementDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovement_StockMovementTypeEnum_MovementType",
                        column: x => x.MovementType,
                        principalSchema: "ECommerce",
                        principalTable: "StockMovementTypeEnum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovement_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "ECommerce",
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "ECommerce",
                table: "ShippingStatusEnum",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Pending", "Pending" },
                    { 2, "Shipped", "Shipped" },
                    { 3, "Delivered", "Delivered" },
                    { 4, "Returned", "Returned" },
                    { 5, "Cancelled", "Cancelled" }
                });

            migrationBuilder.InsertData(
                schema: "ECommerce",
                table: "StockMovementTypeEnum",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Sale", "Sale" },
                    { 2, "Restock", "Restock" },
                    { 3, "Return", "Return" },
                    { 4, "Adjustment", "Adjustment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_ProductId",
                schema: "ECommerce",
                table: "CategoryProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                schema: "ECommerce",
                table: "Customer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                schema: "ECommerce",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "ECommerce",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                schema: "ECommerce",
                table: "OrderItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrace_OrderId",
                schema: "ECommerce",
                table: "OrderTrace",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                schema: "ECommerce",
                table: "Payment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SellerId",
                schema: "ECommerce",
                table: "Product",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                schema: "ECommerce",
                table: "ProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerUser_UserId",
                schema: "ECommerce",
                table: "SellerUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_OrderId",
                schema: "ECommerce",
                table: "Shipping",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_Status",
                schema: "ECommerce",
                table: "Shipping",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingStatusEnum_Name",
                schema: "ECommerce",
                table: "ShippingStatusEnum",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductId",
                schema: "ECommerce",
                table: "Stock",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_MovementType",
                schema: "ECommerce",
                table: "StockMovement",
                column: "MovementType");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_StockId",
                schema: "ECommerce",
                table: "StockMovement",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementTypeEnum_Name",
                schema: "ECommerce",
                table: "StockMovementTypeEnum",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryProduct",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "OrderTrace",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "ProductDetail",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "SellerUser",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Shipping",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "StockMovement",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "ShippingStatusEnum",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "StockMovementTypeEnum",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "ECommerce");

            migrationBuilder.DropTable(
                name: "Seller",
                schema: "ECommerce");
        }
    }
}
