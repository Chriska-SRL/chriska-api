using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class SaleItem
    {
        private int SaleItemId { get; set; }

        private int Quantity { get; set; }
        private decimal UnitPrice { get; set; }

        private Product Product { get; set; }

    }
}
