using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsDistribution
{
    public class DistributionUpdateRequest: AuditableRequest
    {
        public int Id { get; set; }
        public string? Observations { get; set; }
        public int? UserId { get; set; }
        public int? VehicleId { get; set; }
        public List<int>? deliveryIds { get; set; }
    }
}
