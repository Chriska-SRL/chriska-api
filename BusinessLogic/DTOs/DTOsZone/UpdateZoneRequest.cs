using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsZone
{
    public class UpdateZoneRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> DeliveryDays { get; set; } = new List<string>();
        public List<string> RequestDays { get; set; } = new List<string>();
    }
}
