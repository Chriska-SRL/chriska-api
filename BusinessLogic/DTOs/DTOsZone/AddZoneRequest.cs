using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsZone
{
    public class AddZoneRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> DeliveryDays { get; set; } = new List<string>();
        public List<string> RequestDays { get; set; } = new List<string>();

    }
}
