using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Sale
    {
        private int Id { get; set; }
        private DateTime Date { get; set; }
        private string Status { get; set; }

        private SaleItem SaleItem { get; set; }
    }
}
