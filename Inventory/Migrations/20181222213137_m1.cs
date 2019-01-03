using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SerieID = table.Column<string>(nullable: false),
                    LastUsedNumber = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    Table = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SerieID);
                });

            migrationBuilder.CreateTable(
                name: "StockKeepUnits",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    QuantityPerUnit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockKeepUnits", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseName = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseName);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseHeaders",
                columns: table => new
                {
                    DocumentID = table.Column<string>(nullable: false),
                    CustomerID = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseHeaders", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_ReleaseHeaders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveHeaders",
                columns: table => new
                {
                    DocumentID = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ReceiveDate = table.Column<DateTime>(nullable: false),
                    VendorID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveHeaders", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_ReceiveHeaders_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<string>(nullable: false),
                    DefaultWarehouse = table.Column<string>(nullable: true),
                    DefaultWarehousePlace = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_Warehouses_DefaultWarehouse",
                        column: x => x.DefaultWarehouse,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryHeaders",
                columns: table => new
                {
                    DocumentID = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    InventoryDate = table.Column<DateTime>(nullable: false),
                    WarehouseName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryHeaders", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_InventoryHeaders_Warehouses_WarehouseName",
                        column: x => x.WarehouseName,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehousePlaces",
                columns: table => new
                {
                    WarehouseName = table.Column<string>(nullable: false),
                    WarehousePlaceName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehousePlaces", x => new { x.WarehouseName, x.WarehousePlaceName });
                    table.ForeignKey(
                        name: "FK_WarehousePlaces_Warehouses_WarehouseName",
                        column: x => x.WarehouseName,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemID = table.Column<string>(nullable: false),
                    CategoryID = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryLines",
                columns: table => new
                {
                    DocumentID = table.Column<string>(nullable: false),
                    PositionNumber = table.Column<int>(nullable: false),
                    CountedQuantity = table.Column<double>(nullable: false),
                    ItemID = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    StockKeepUnit = table.Column<string>(nullable: true),
                    WarehouseNumber = table.Column<string>(nullable: true),
                    WarehousePlace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryLines", x => new { x.DocumentID, x.PositionNumber });
                    table.ForeignKey(
                        name: "FK_InventoryLines_InventoryHeaders_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "InventoryHeaders",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryLines_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryLines_StockKeepUnits_StockKeepUnit",
                        column: x => x.StockKeepUnit,
                        principalTable: "StockKeepUnits",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryLines_Warehouses_WarehouseNumber",
                        column: x => x.WarehouseNumber,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemStockKeepUnits",
                columns: table => new
                {
                    ItemID = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Barcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStockKeepUnits", x => new { x.ItemID, x.Code });
                    table.ForeignKey(
                        name: "FK_ItemStockKeepUnits_StockKeepUnits_Code",
                        column: x => x.Code,
                        principalTable: "StockKeepUnits",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemStockKeepUnits_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveLines",
                columns: table => new
                {
                    DocumentID = table.Column<string>(nullable: false),
                    PositionNumber = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemID = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ReceiveQuantity = table.Column<double>(nullable: false),
                    ReceivedQuantity = table.Column<double>(nullable: false),
                    StockKeepUnit = table.Column<string>(nullable: true),
                    WarehouseNumber = table.Column<string>(nullable: true),
                    WarehousePlace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveLines", x => new { x.DocumentID, x.PositionNumber });
                    table.ForeignKey(
                        name: "FK_ReceiveLines_ReceiveHeaders_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "ReceiveHeaders",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveLines_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiveLines_StockKeepUnits_StockKeepUnit",
                        column: x => x.StockKeepUnit,
                        principalTable: "StockKeepUnits",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiveLines_Warehouses_WarehouseNumber",
                        column: x => x.WarehouseNumber,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseLines",
                columns: table => new
                {
                    DocumentID = table.Column<string>(nullable: false),
                    PositionNumber = table.Column<int>(nullable: false),
                    ItemID = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ReleaseQuantity = table.Column<double>(nullable: false),
                    ReleasedQuantity = table.Column<double>(nullable: false),
                    StockKeepUnit = table.Column<string>(nullable: true),
                    WarehouseNumber = table.Column<string>(nullable: true),
                    WarehousePlace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseLines", x => new { x.DocumentID, x.PositionNumber });
                    table.ForeignKey(
                        name: "FK_ReleaseLines_ReleaseHeaders_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "ReleaseHeaders",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseLines_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReleaseLines_StockKeepUnits_StockKeepUnit",
                        column: x => x.StockKeepUnit,
                        principalTable: "StockKeepUnits",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReleaseLines_Warehouses_WarehouseNumber",
                        column: x => x.WarehouseNumber,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseEntries",
                columns: table => new
                {
                    EntryNumber = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerID = table.Column<string>(nullable: true),
                    DocumentDate = table.Column<DateTime>(nullable: false),
                    DocumentDescription = table.Column<string>(nullable: true),
                    DocumentNumber = table.Column<string>(nullable: true),
                    EntryType = table.Column<int>(nullable: false),
                    ItemID = table.Column<string>(nullable: true),
                    KeepUnit = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    QuantityPerUnit = table.Column<double>(nullable: false),
                    TotalQuantity = table.Column<double>(nullable: false),
                    VendorID = table.Column<string>(nullable: true),
                    WarehouseNumber = table.Column<string>(nullable: true),
                    WarehousePlace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseEntries", x => x.EntryNumber);
                    table.ForeignKey(
                        name: "FK_WarehouseEntries_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseEntries_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseEntries_StockKeepUnits_KeepUnit",
                        column: x => x.KeepUnit,
                        principalTable: "StockKeepUnits",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseEntries_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseEntries_Warehouses_WarehouseNumber",
                        column: x => x.WarehouseNumber,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DefaultWarehouse",
                table: "Categories",
                column: "DefaultWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryHeaders_WarehouseName",
                table: "InventoryHeaders",
                column: "WarehouseName");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLines_ItemID",
                table: "InventoryLines",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLines_StockKeepUnit",
                table: "InventoryLines",
                column: "StockKeepUnit");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLines_WarehouseNumber",
                table: "InventoryLines",
                column: "WarehouseNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryID",
                table: "Items",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStockKeepUnits_Code",
                table: "ItemStockKeepUnits",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveHeaders_VendorID",
                table: "ReceiveHeaders",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveLines_ItemID",
                table: "ReceiveLines",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveLines_StockKeepUnit",
                table: "ReceiveLines",
                column: "StockKeepUnit");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveLines_WarehouseNumber",
                table: "ReceiveLines",
                column: "WarehouseNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseHeaders_CustomerID",
                table: "ReleaseHeaders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseLines_ItemID",
                table: "ReleaseLines",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseLines_StockKeepUnit",
                table: "ReleaseLines",
                column: "StockKeepUnit");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseLines_WarehouseNumber",
                table: "ReleaseLines",
                column: "WarehouseNumber");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEntries_CustomerID",
                table: "WarehouseEntries",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEntries_ItemID",
                table: "WarehouseEntries",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEntries_KeepUnit",
                table: "WarehouseEntries",
                column: "KeepUnit");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEntries_VendorID",
                table: "WarehouseEntries",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEntries_WarehouseNumber",
                table: "WarehouseEntries",
                column: "WarehouseNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryLines");

            migrationBuilder.DropTable(
                name: "ItemStockKeepUnits");

            migrationBuilder.DropTable(
                name: "ReceiveLines");

            migrationBuilder.DropTable(
                name: "ReleaseLines");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "WarehouseEntries");

            migrationBuilder.DropTable(
                name: "WarehousePlaces");

            migrationBuilder.DropTable(
                name: "InventoryHeaders");

            migrationBuilder.DropTable(
                name: "ReceiveHeaders");

            migrationBuilder.DropTable(
                name: "ReleaseHeaders");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "StockKeepUnits");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
