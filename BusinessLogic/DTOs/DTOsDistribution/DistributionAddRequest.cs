using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDistribution
{
    public class DistributionAddRequest: AuditableRequest
    {
        public string Observations { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public List<int> ZoneIds { get; set; }
        public List<int> ClientIds { get; set; }
    }
}
