using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Purchase
    {
        private int Id { get; set; }
        private DateTime Date { get; set; }
        private string Status { get; set; }
        private Supplier Supplier { get; set; }
        private List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    }
}
