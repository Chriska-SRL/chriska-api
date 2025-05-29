using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class ProductResponse
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public SubCategoryResponse SubCategory { get; set; }
       

    }
}
