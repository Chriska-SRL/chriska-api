using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Vehicle
    {
        private int Vehicleid;
        private string Plate { get; set; }
        private string Brand { get; set; }
        private string Model { get; set; }
        private int CrateCapacity { get; set; }
        private Delivery Delivery { get; set; }


    }
}
