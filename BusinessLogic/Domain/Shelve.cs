using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Shelve
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
        public List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
