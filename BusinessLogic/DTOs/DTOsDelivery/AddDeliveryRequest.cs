using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class AddDeliveryRequest
    {
        public DateTime Date { get; set; }
        public string DriverName { get; set; }
        public string Observation { get; set; }
        public int VehicleId { get; set; }

    }
}
