using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class StockMovement
    {
        private int Id { get; set; }
        private DateTime Date { get; set; }
        private int Quantity { get; set; }
        private string Type { get; set; }
        private string Reason { get; set; }
        private Shelve Shelve { get; set; }
        private User User { get; set; }
    }
}
