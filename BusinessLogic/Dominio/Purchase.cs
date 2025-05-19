using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Purchase
    {
        private int PurchaseId { get; set; }
        private DateTime PurchaseDate { get; set; }
        private string Status { get; set; }
        private Supplier Supplier { get; set; }
        private List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    }
}
