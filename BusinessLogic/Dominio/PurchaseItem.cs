using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class PurchaseItem
    {
        private int PurchaseItemId { get; set; }
        private int Quantity { get; set; }
        private decimal UnitPrice { get; set; }
        private Product Product { get; set; }
    }
}
