using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_ProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SupplierAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContactFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Contacts_ContactsId",
                        column: x => x.ContactsId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FkProductCategory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_Products_tb_ProductCategories_FkProductCategory",
                        column: x => x.FkProductCategory,
                        principalTable: "tb_ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FkSupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuppliersId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomersId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CHK_Orders_FkCustomerID", "(OrderType = 'Sale' AND FkCustomerID IS NOT NULL AND FkSupplierID IS NULL) OR\n              (OrderType = 'Purchase' AND FkSupplierID IS NOT NULL AND FkCustomerID IS NULL)");
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_tb_Suppliers_SuppliersId",
                        column: x => x.SuppliersId,
                        principalTable: "tb_Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    MinStockLevel = table.Column<int>(type: "int", nullable: false),
                    MaxStockLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_Inventory_tb_Products_FkProductId",
                        column: x => x.FkProductId,
                        principalTable: "tb_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_ProductSuppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkSupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ProductSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_ProductSuppliers_tb_Products_FkProductId",
                        column: x => x.FkProductId,
                        principalTable: "tb_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_ProductSuppliers_tb_Suppliers_FkSupplierId",
                        column: x => x.FkSupplierId,
                        principalTable: "tb_Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_tb_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tb_Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                table: "Contacts",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContactsId",
                table: "Customers",
                column: "ContactsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomersId",
                table: "Orders",
                column: "CustomersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SuppliersId",
                table: "Orders",
                column: "SuppliersId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Inventory_FkProductId",
                table: "tb_Inventory",
                column: "FkProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_ProductCategories_ProductCategoryName",
                table: "tb_ProductCategories",
                column: "ProductCategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_Products_FkProductCategory",
                table: "tb_Products",
                column: "FkProductCategory");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Products_ProductName",
                table: "tb_Products",
                column: "ProductName",
                unique: true,
                filter: "[ProductName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ProductSuppliers_FkProductId_FkSupplierId",
                table: "tb_ProductSuppliers",
                columns: new[] { "FkProductId", "FkSupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_ProductSuppliers_FkSupplierId",
                table: "tb_ProductSuppliers",
                column: "FkSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Suppliers_ContactEmail",
                table: "tb_Suppliers",
                column: "ContactEmail");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Suppliers_SupplierName",
                table: "tb_Suppliers",
                column: "SupplierName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "tb_Inventory");

            migrationBuilder.DropTable(
                name: "tb_ProductSuppliers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "tb_Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "tb_Suppliers");

            migrationBuilder.DropTable(
                name: "tb_ProductCategories");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
