using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class ReleaseHeader
    {
        public ReleaseHeader()
        {
        }

        public ReleaseHeader(string ID)
        {
            DocumentID = InvMgt.GetNextReleaseNo();
        }

        public ReleaseHeader(string documentID, string description, DateTime releaseDate, string customerID)
        {
            DocumentID = documentID;
            Description = description;
            ReleaseDate = releaseDate;
            CustomerID = customerID;
        }

        public string DocumentID { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CustomerID { get; set; }


        public Customer Customer { get; set; }

        public List<ReleaseLine> ReleaseLines { get; set; }
    }
}
