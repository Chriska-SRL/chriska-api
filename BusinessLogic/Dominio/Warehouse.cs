using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Warehouse
    {
        private int WarehouseId { get; set; }
        private string Name { get; set; }
        private string Description { get; set; }
        private string Address { get; set; }
        private List<Shelve> Shelves { get; set; } = new List<Shelve>();
    }
}
