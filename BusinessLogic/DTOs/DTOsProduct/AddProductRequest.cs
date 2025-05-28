using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class AddProductRequest
    {
        public string InternalCode { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public string UnitType { get; set; }
        public string Description { get; set; }
        public string TemperatureCondition { get; set; }
        public string Observation { get; set; }
        public int SubCategoryId { get; set; }
    }
}
