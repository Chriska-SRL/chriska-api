using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class StockMovement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public Shelve Shelve { get; set; }
        public User User { get; set; }
    }
}
