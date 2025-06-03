using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsProductStock
{
    public class ProductStockResponse
    {
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; }
    }
}
