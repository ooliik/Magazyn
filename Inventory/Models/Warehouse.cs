using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class Warehouse
    {
        public Warehouse()
        {
        }

        public Warehouse(Warehouse warehouse)
        {
            WarehouseName = warehouse.WarehouseName;
            Description = warehouse.Description;
            Address = warehouse.Address;
        }

        public Warehouse(string warehouseName, string description, string address)
        {
            WarehouseName = warehouseName;
            Description = description;
            Address = address;
        }


        public string WarehouseName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }


        public List<WarehouseEntry> WarehouseEnrties { get; set; }
        public List<WarehousePlace> WarehousePlaces { get; set; }
        public List<ReleaseLine> ReleaseLines { get; set; }
        public List<ReceiveLine> ReceiveLines { get; set; }
        public List<InventoryLine> InventoryLines { get; set; }
        public List<InventoryHeader> InventoryHeaders { get; set; }
        public List<Category> Categories { get; set; }
    }
}
