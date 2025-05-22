using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsVehicle
{
    public class AddVehicleRequest
    {
        private string Plate { get; set; }
        private string Brand { get; set; }
        private string Model { get; set; }
        private int CrateCapacity { get; set; }
        private int DeliveryId { get; set; }

    }
}
