using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsVehicle
{
    public class AddVehicleRequest
    {
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public int DeliveryId { get; set; }

    }
}
