using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StockKeepUnit> StockKeepUnits { get; set; }
        public DbSet<ItemStockKeepUnit> ItemStockKeepUnits { get; set; }
        public DbSet<WarehouseEntry> WarehouseEntries { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ReleaseHeader> ReleaseHeaders { get; set; }
        public DbSet<ReleaseLine> ReleaseLines { get; set; }
        public DbSet<WarehousePlace> WarehousePlaces { get; set; }
        public DbSet<ReceiveHeader> ReceiveHeaders { get; set; }
        public DbSet<ReceiveLine> ReceiveLines { get; set; }
        public DbSet<InventoryHeader> InventoryHeaders { get; set; }
        public DbSet<InventoryLine> InventoryLines { get; set; }
        public DbSet<Serie> Series { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Warehouse.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemStockKeepUnit>()
                .HasKey(ba => new { ba.ItemID, ba.Code });

            modelBuilder.Entity<Category>()
                .HasKey(ba => new { ba.CategoryID });

            modelBuilder.Entity<Item>()
                .HasKey(ba => new { ba.ItemID });

            modelBuilder.Entity<StockKeepUnit>()
                .HasKey(ba => new { ba.Code });

            modelBuilder.Entity<WarehouseEntry>()
                .HasKey(bd => new { bd.EntryNumber });

            modelBuilder.Entity<Warehouse>()
                .HasKey(ba => new { ba.WarehouseName });

            modelBuilder.Entity<Customer>()
                .HasKey(ba => new { ba.CustomerID });

            modelBuilder.Entity<Vendor>()
                .HasKey(ba => new { ba.VendorID });

            modelBuilder.Entity<ReleaseHeader>()
                .HasKey(ba => new { ba.DocumentID });

            modelBuilder.Entity<ReleaseLine>()
                .HasKey(ba => new { ba.DocumentID, ba.PositionNumber });

            modelBuilder.Entity<WarehousePlace>()
                .HasKey(ba => new { ba.WarehouseName, ba.WarehousePlaceName });

            modelBuilder.Entity<ReceiveHeader>()
                .HasKey(ba => new { ba.DocumentID });

            modelBuilder.Entity<ReceiveLine>()
                .HasKey(ba => new { ba.DocumentID, ba.PositionNumber });

            modelBuilder.Entity<InventoryHeader>()
                .HasKey(ba => new { ba.DocumentID });

            modelBuilder.Entity<InventoryLine>()
                .HasKey(ba => new { ba.DocumentID, ba.PositionNumber });

            modelBuilder.Entity<Serie>()
                .HasKey(ba => new { ba.SerieID });

            modelBuilder.Entity<Category>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.Categories)
                .HasForeignKey(ba => ba.DefaultWarehouse);

            modelBuilder.Entity<Item>()
               .HasOne(bb => bb.Category)
               .WithMany(a => a.Items)
               .HasForeignKey(bb => bb.CategoryID);

            modelBuilder.Entity<ItemStockKeepUnit>()
                .HasOne(ba => ba.Item)
                .WithMany(b => b.ItemStockKeepUnits)
                .HasForeignKey(ba => ba.ItemID);

            modelBuilder.Entity<ItemStockKeepUnit>()
               .HasOne(ba => ba.StockKeepUnit)
                .WithMany(b => b.ItemStockKeepUnits)
                .HasForeignKey(ba => ba.Code);

            modelBuilder.Entity<WarehouseEntry>()
               .HasOne(ba => ba.Item)
                .WithMany(b => b.WarehouseEnrties)
                .HasForeignKey(ba => ba.ItemID);

            modelBuilder.Entity<WarehouseEntry>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.WarehouseEnrties)
                .HasForeignKey(ba => ba.WarehouseNumber);

            modelBuilder.Entity<WarehouseEntry>()
                .HasOne(ba => ba.StockKeepUnit)
                .WithMany(b => b.WarehouseEntries)
                .HasForeignKey(ba => ba.KeepUnit);

            modelBuilder.Entity<WarehouseEntry>()
                .HasOne(ba => ba.Customer)
                .WithMany(b => b.WarehouseEntries)
                .HasForeignKey(ba => ba.CustomerID);

            modelBuilder.Entity<WarehouseEntry>()
                .HasOne(ba => ba.Vendor)
                .WithMany(b => b.WarehouseEntries)
                .HasForeignKey(ba => ba.VendorID);

            modelBuilder.Entity<ReleaseLine>()
                .HasOne(ba => ba.ReleaseHeader)
                .WithMany(b => b.ReleaseLines)
                .HasForeignKey(ba => ba.DocumentID);

            modelBuilder.Entity<ReleaseLine>()
                .HasOne(ba => ba.Item)
                .WithMany(b => b.ReleaseLines)
                .HasForeignKey(ba => ba.ItemID);

            modelBuilder.Entity<ReleaseLine>()
                .HasOne(ba => ba.StockKeepUnitt)
                .WithMany(b => b.ReleaseLines)
                .HasForeignKey(ba => ba.StockKeepUnit);

            modelBuilder.Entity<ReleaseLine>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.ReleaseLines)
                .HasForeignKey(ba => ba.WarehouseNumber);

            modelBuilder.Entity<ReleaseHeader>()
               .HasOne(ba => ba.Customer)
               .WithMany(b => b.ReleaseHeaders)
               .HasForeignKey(ba => ba.CustomerID);

            modelBuilder.Entity<WarehousePlace>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.WarehousePlaces)
                .HasForeignKey(ba => ba.WarehouseName);

            modelBuilder.Entity<ReceiveLine>()
               .HasOne(ba => ba.ReceiveHeader)
               .WithMany(b => b.ReceiveLines)
               .HasForeignKey(ba => ba.DocumentID);

            modelBuilder.Entity<ReceiveLine>()
                .HasOne(ba => ba.Item)
                .WithMany(b => b.ReceiveLines)
                .HasForeignKey(ba => ba.ItemID);

            modelBuilder.Entity<ReceiveLine>()
                .HasOne(ba => ba.StockKeepUnitt)
                .WithMany(b => b.ReceiveLines)
                .HasForeignKey(ba => ba.StockKeepUnit);

            modelBuilder.Entity<ReceiveLine>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.ReceiveLines)
                .HasForeignKey(ba => ba.WarehouseNumber);

            modelBuilder.Entity<ReceiveHeader>()
               .HasOne(ba => ba.Vendor)
               .WithMany(b => b.ReceiveHeaders)
               .HasForeignKey(ba => ba.VendorID);

            modelBuilder.Entity<InventoryHeader>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.InventoryHeaders)
                .HasForeignKey(ba => ba.WarehouseName);


            modelBuilder.Entity<InventoryLine>()
                .HasOne(ba => ba.InventoryHeader)
                .WithMany(b => b.InventoryLines)
                .HasForeignKey(ba => ba.DocumentID);

            modelBuilder.Entity<InventoryLine>()
                .HasOne(ba => ba.Item)
                .WithMany(b => b.InventoryLines)
                .HasForeignKey(ba => ba.ItemID);

            modelBuilder.Entity<InventoryLine>()
                .HasOne(ba => ba.StockKeepUnitt)
                .WithMany(b => b.InventoryLines)
                .HasForeignKey(ba => ba.StockKeepUnit);

            modelBuilder.Entity<InventoryLine>()
                .HasOne(ba => ba.Warehouse)
                .WithMany(b => b.InventoryLines)
                .HasForeignKey(ba => ba.WarehouseNumber);
        }
    }
}
