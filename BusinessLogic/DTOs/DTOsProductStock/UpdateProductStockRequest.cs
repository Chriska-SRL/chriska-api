using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsProductStock
{
    public class UpdateProductStockRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
