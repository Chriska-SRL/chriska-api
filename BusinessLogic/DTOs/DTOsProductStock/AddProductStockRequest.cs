using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsProductStock
{
    public class AddProductStockRequest
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
