using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsOrderItem
{
    public class AddOrderItemRequest
    {
        private int Quantity { get; set; }
        private decimal UnitPrice { get; set; }
        private int ProductId { get; set; }
    }
}
