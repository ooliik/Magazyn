using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class Category
    {
        public Category()
        {
        }

        public Category(string categoryName, string description, string defaultWarehouse, string defaultWarehousePlace)
        {
            CategoryID = categoryName;
            Description = description;
            DefaultWarehouse = defaultWarehouse;
            DefaultWarehousePlace = defaultWarehousePlace;
        }
        public Category(string categoryName, string description)
        {
            CategoryID = categoryName;
            Description = description;
        }

        public string CategoryID { get; set; }
        public string Description { get; set; }
        public string DefaultWarehouse { get; set; }
        public string DefaultWarehousePlace { get; set; }

        public List<Item> Items { get; set; }

        public Warehouse Warehouse { get; set; }

    }
}
