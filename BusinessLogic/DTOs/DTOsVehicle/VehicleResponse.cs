using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsVehicle
{
    public class VehicleResponse
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public CostResponse Cost { get; set; }

    }
}
