using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Delivery
    {
        private int Id { get; set; }
        private DateTime Date { get; set; }
        private string DriverName { get; set; }
        private string Observation { get; set; }
        private List<Order> Orders { get; set; } = new List<Order>();
        private Cost Cost { get; set; }
        private List<Zone> Zones { get; set; } = new List<Zone>();
        private Vehicle Vehicle { get; set; }


    }
}
