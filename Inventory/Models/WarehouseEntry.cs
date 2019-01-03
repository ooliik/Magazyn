using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public enum EntryType
    {
        Correction,
        Release,
        Receive
    }

    public class WarehouseEntry
    {

        public WarehouseEntry()
        {
            using (DBContext dBCtx = new DBContext())
            {
                EntryNumber = dBCtx.WarehouseEntries.Count() + 1;

            }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryNumber { get; set; }
        public string CustomerID { get; set; }
        public string VendorID { get; set; }
        public string ItemID { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentDescription { get; set; }
        public EntryType EntryType { get; set; }
        public double TotalQuantity { get; set; }
        public double Quantity { get; set; }
        public double QuantityPerUnit { get; set; }
        public string KeepUnit { get; set; }
        public string WarehouseNumber { get; set; }
        public string WarehousePlace { get; set; }



        public Item Item { get; set; }
        public Warehouse Warehouse { get; set; }
        public StockKeepUnit StockKeepUnit { get; set; }
        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
    }
}
