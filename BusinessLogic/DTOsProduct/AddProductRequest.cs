using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsProduct
{
    public class AddProductRequest
    {
        private string InternalCode { get; set; }
        private string Barcode { get; set; }
        private string Name { get; set; }
        private decimal Price { get; set; }
        private string Image { get; set; }
        private int Stock { get; set; }
        private string UnitType { get; set; }
        private string TemperatureCondition { get; set; }
        private string Observation { get; set; }
        private int SubCategoryId { get; set; }
    }
}
