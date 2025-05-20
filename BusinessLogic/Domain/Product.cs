using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Product
    {
        private int Id { get; set; }
        private string InternalCode { get; set; }
        private string Barcode { get; set; }
        private string Name { get; set; }
        private decimal Price { get; set; }
        private string Image { get; set; }
        private int Stock { get; set; }
        private string UnitType { get; set; }
        private string TemperatureCondition { get; set; }
        private string Observation { get; set; }
        private SubCategory SubCategory { get; set; }
        private List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
