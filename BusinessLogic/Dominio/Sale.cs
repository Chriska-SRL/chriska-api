using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Sale
    {
        private int SaleId { get; set; }
        private DateTime SaleDate { get; set; }
        private string Status { get; set; }

        private SaleItem SaleItem { get; set; }
    }
}
