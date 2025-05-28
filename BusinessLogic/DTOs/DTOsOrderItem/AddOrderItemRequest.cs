using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsOrderItem
{
    public class AddOrderItemRequest
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
    }
}
