using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Product
    {
        public int Id { get; set; }
        public string InternalCode { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public string UnitType { get; set; }
        public string TemperatureCondition { get; set; }
        public string Observation { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
