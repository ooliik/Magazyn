using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class InventoryHeader
    {
        public InventoryHeader()
        {
        }

        public InventoryHeader(string ID)
        {
            DocumentID = InvMgt.GetNextInventoryHeader();
        }

        public InventoryHeader(string documentID, string description, DateTime inventoryDate, string warehouseName)
        {
            DocumentID = documentID;
            Description = description;
            InventoryDate = inventoryDate;
            WarehouseName = warehouseName;
        }

        public string DocumentID { get; set; }
        public string Description { get; set; }
        public DateTime InventoryDate { get; set; }
        public string WarehouseName { get; set; }

        public Warehouse Warehouse { get; set; }

        public List<InventoryLine> InventoryLines { get; set; }
    }
}
