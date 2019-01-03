using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public enum TableType
    {
        Item,
        Vendor,
        Customer,
        ReleaseHeader,
        ReceiveHeader,
        InventoryHeader
    }
    public class Serie
    {
        public string SerieID { get; set; }
        public string Prefix { get; set; }
        public string LastUsedNumber { get; set; }
        public TableType Table { get; set; }

        public Serie()
        {

        }

        public Serie(string serieID, string prefix, string lastUsedNumber, TableType table)
        {
            SerieID = serieID;
            Prefix = prefix;
            LastUsedNumber = lastUsedNumber;
            Table = table;
        }


    }
}
