using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Shelve
    {
        private int ShelveId { get; set; }
        private string Description { get; set; }
        private Warehouse Warehouse { get; set; }
        private List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
        private List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
