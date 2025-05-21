using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class OrderItem
    {
        private int Id { get; set; }
        private int Quantity { get; set; }
        private decimal UnitPrice { get; set; }
        private Product Product { get; set; }
       
    }

}
