using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DriverName { get; set; }
        public string Observation { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public Cost Cost { get; set; }
        public List<Zone> Zones { get; set; } = new List<Zone>();
        public Vehicle Vehicle { get; set; }


    }
}
