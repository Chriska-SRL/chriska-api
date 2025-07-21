using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsVehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class DeliveryResponse : AuditableResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DriverName { get; set; }
        public string Observation { get; set; }
        public VehicleResponse Vehicle { get; set; }
    }
}
