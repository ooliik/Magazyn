using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Inventory.Models;

namespace Inventory.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20181222213137_m1")]
    partial class m1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3");

            modelBuilder.Entity("Inventory.Models.Category", b =>
                {
                    b.Property<string>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DefaultWarehouse");

                    b.Property<string>("DefaultWarehousePlace");

                    b.Property<string>("Description");

                    b.HasKey("CategoryID");

                    b.HasIndex("DefaultWarehouse");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Inventory.Models.Customer", b =>
                {
                    b.Property<string>("CustomerID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Inventory.Models.InventoryHeader", b =>
                {
                    b.Property<string>("DocumentID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("InventoryDate");

                    b.Property<string>("WarehouseName");

                    b.HasKey("DocumentID");

                    b.HasIndex("WarehouseName");

                    b.ToTable("InventoryHeaders");
                });

            modelBuilder.Entity("Inventory.Models.InventoryLine", b =>
                {
                    b.Property<string>("DocumentID");

                    b.Property<int>("PositionNumber");

                    b.Property<double>("CountedQuantity");

                    b.Property<string>("ItemID");

                    b.Property<double>("Quantity");

                    b.Property<string>("StockKeepUnit");

                    b.Property<string>("WarehouseNumber");

                    b.Property<string>("WarehousePlace");

                    b.HasKey("DocumentID", "PositionNumber");

                    b.HasIndex("ItemID");

                    b.HasIndex("StockKeepUnit");

                    b.HasIndex("WarehouseNumber");

                    b.ToTable("InventoryLines");
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.Property<string>("ItemID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryID");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ItemID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Inventory.Models.ItemStockKeepUnit", b =>
                {
                    b.Property<string>("ItemID");

                    b.Property<string>("Code");

                    b.Property<string>("Barcode");

                    b.HasKey("ItemID", "Code");

                    b.HasIndex("Code");

                    b.ToTable("ItemStockKeepUnits");
                });

            modelBuilder.Entity("Inventory.Models.ReceiveHeader", b =>
                {
                    b.Property<string>("DocumentID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("ReceiveDate");

                    b.Property<string>("VendorID");

                    b.HasKey("DocumentID");

                    b.HasIndex("VendorID");

                    b.ToTable("ReceiveHeaders");
                });

            modelBuilder.Entity("Inventory.Models.ReceiveLine", b =>
                {
                    b.Property<string>("DocumentID");

                    b.Property<int>("PositionNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ItemID");

                    b.Property<double>("Quantity");

                    b.Property<double>("ReceiveQuantity");

                    b.Property<double>("ReceivedQuantity");

                    b.Property<string>("StockKeepUnit");

                    b.Property<string>("WarehouseNumber");

                    b.Property<string>("WarehousePlace");

                    b.HasKey("DocumentID", "PositionNumber");

                    b.HasIndex("ItemID");

                    b.HasIndex("StockKeepUnit");

                    b.HasIndex("WarehouseNumber");

                    b.ToTable("ReceiveLines");
                });

            modelBuilder.Entity("Inventory.Models.ReleaseHeader", b =>
                {
                    b.Property<string>("DocumentID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerID");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ReleaseDate");

                    b.HasKey("DocumentID");

                    b.HasIndex("CustomerID");

                    b.ToTable("ReleaseHeaders");
                });

            modelBuilder.Entity("Inventory.Models.ReleaseLine", b =>
                {
                    b.Property<string>("DocumentID");

                    b.Property<int>("PositionNumber");

                    b.Property<string>("ItemID");

                    b.Property<double>("Quantity");

                    b.Property<double>("ReleaseQuantity");

                    b.Property<double>("ReleasedQuantity");

                    b.Property<string>("StockKeepUnit");

                    b.Property<string>("WarehouseNumber");

                    b.Property<string>("WarehousePlace");

                    b.HasKey("DocumentID", "PositionNumber");

                    b.HasIndex("ItemID");

                    b.HasIndex("StockKeepUnit");

                    b.HasIndex("WarehouseNumber");

                    b.ToTable("ReleaseLines");
                });

            modelBuilder.Entity("Inventory.Models.Serie", b =>
                {
                    b.Property<string>("SerieID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LastUsedNumber");

                    b.Property<string>("Prefix");

                    b.Property<int>("Table");

                    b.HasKey("SerieID");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("Inventory.Models.StockKeepUnit", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("QuantityPerUnit");

                    b.HasKey("Code");

                    b.ToTable("StockKeepUnits");
                });

            modelBuilder.Entity("Inventory.Models.Vendor", b =>
                {
                    b.Property<string>("VendorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.HasKey("VendorID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Inventory.Models.Warehouse", b =>
                {
                    b.Property<string>("WarehouseName")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Description");

                    b.HasKey("WarehouseName");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Inventory.Models.WarehouseEntry", b =>
                {
                    b.Property<int>("EntryNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerID");

                    b.Property<DateTime>("DocumentDate");

                    b.Property<string>("DocumentDescription");

                    b.Property<string>("DocumentNumber");

                    b.Property<int>("EntryType");

                    b.Property<string>("ItemID");

                    b.Property<string>("KeepUnit");

                    b.Property<double>("Quantity");

                    b.Property<double>("QuantityPerUnit");

                    b.Property<double>("TotalQuantity");

                    b.Property<string>("VendorID");

                    b.Property<string>("WarehouseNumber");

                    b.Property<string>("WarehousePlace");

                    b.HasKey("EntryNumber");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ItemID");

                    b.HasIndex("KeepUnit");

                    b.HasIndex("VendorID");

                    b.HasIndex("WarehouseNumber");

                    b.ToTable("WarehouseEntries");
                });

            modelBuilder.Entity("Inventory.Models.WarehousePlace", b =>
                {
                    b.Property<string>("WarehouseName");

                    b.Property<string>("WarehousePlaceName");

                    b.HasKey("WarehouseName", "WarehousePlaceName");

                    b.ToTable("WarehousePlaces");
                });

            modelBuilder.Entity("Inventory.Models.Category", b =>
                {
                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("Categories")
                        .HasForeignKey("DefaultWarehouse");
                });

            modelBuilder.Entity("Inventory.Models.InventoryHeader", b =>
                {
                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("InventoryHeaders")
                        .HasForeignKey("WarehouseName");
                });

            modelBuilder.Entity("Inventory.Models.InventoryLine", b =>
                {
                    b.HasOne("Inventory.Models.InventoryHeader", "InventoryHeader")
                        .WithMany("InventoryLines")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inventory.Models.Item", "Item")
                        .WithMany("InventoryLines")
                        .HasForeignKey("ItemID");

                    b.HasOne("Inventory.Models.StockKeepUnit", "StockKeepUnitt")
                        .WithMany("InventoryLines")
                        .HasForeignKey("StockKeepUnit");

                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("InventoryLines")
                        .HasForeignKey("WarehouseNumber");
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.HasOne("Inventory.Models.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryID");
                });

            modelBuilder.Entity("Inventory.Models.ItemStockKeepUnit", b =>
                {
                    b.HasOne("Inventory.Models.StockKeepUnit", "StockKeepUnit")
                        .WithMany("ItemStockKeepUnits")
                        .HasForeignKey("Code")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inventory.Models.Item", "Item")
                        .WithMany("ItemStockKeepUnits")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inventory.Models.ReceiveHeader", b =>
                {
                    b.HasOne("Inventory.Models.Vendor", "Vendor")
                        .WithMany("ReceiveHeaders")
                        .HasForeignKey("VendorID");
                });

            modelBuilder.Entity("Inventory.Models.ReceiveLine", b =>
                {
                    b.HasOne("Inventory.Models.ReceiveHeader", "ReceiveHeader")
                        .WithMany("ReceiveLines")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inventory.Models.Item", "Item")
                        .WithMany("ReceiveLines")
                        .HasForeignKey("ItemID");

                    b.HasOne("Inventory.Models.StockKeepUnit", "StockKeepUnitt")
                        .WithMany("ReceiveLines")
                        .HasForeignKey("StockKeepUnit");

                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("ReceiveLines")
                        .HasForeignKey("WarehouseNumber");
                });

            modelBuilder.Entity("Inventory.Models.ReleaseHeader", b =>
                {
                    b.HasOne("Inventory.Models.Customer", "Customer")
                        .WithMany("ReleaseHeaders")
                        .HasForeignKey("CustomerID");
                });

            modelBuilder.Entity("Inventory.Models.ReleaseLine", b =>
                {
                    b.HasOne("Inventory.Models.ReleaseHeader", "ReleaseHeader")
                        .WithMany("ReleaseLines")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inventory.Models.Item", "Item")
                        .WithMany("ReleaseLines")
                        .HasForeignKey("ItemID");

                    b.HasOne("Inventory.Models.StockKeepUnit", "StockKeepUnitt")
                        .WithMany("ReleaseLines")
                        .HasForeignKey("StockKeepUnit");

                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("ReleaseLines")
                        .HasForeignKey("WarehouseNumber");
                });

            modelBuilder.Entity("Inventory.Models.WarehouseEntry", b =>
                {
                    b.HasOne("Inventory.Models.Customer", "Customer")
                        .WithMany("WarehouseEntries")
                        .HasForeignKey("CustomerID");

                    b.HasOne("Inventory.Models.Item", "Item")
                        .WithMany("WarehouseEnrties")
                        .HasForeignKey("ItemID");

                    b.HasOne("Inventory.Models.StockKeepUnit", "StockKeepUnit")
                        .WithMany("WarehouseEntries")
                        .HasForeignKey("KeepUnit");

                    b.HasOne("Inventory.Models.Vendor", "Vendor")
                        .WithMany("WarehouseEntries")
                        .HasForeignKey("VendorID");

                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("WarehouseEnrties")
                        .HasForeignKey("WarehouseNumber");
                });

            modelBuilder.Entity("Inventory.Models.WarehousePlace", b =>
                {
                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("WarehousePlaces")
                        .HasForeignKey("WarehouseName")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
