using BusinessLogic.DTOs.DTOsProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsPurchaseItem
{
    public class PurchaseItemResponse
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductResponse Product{ get; set; }
    }
}
