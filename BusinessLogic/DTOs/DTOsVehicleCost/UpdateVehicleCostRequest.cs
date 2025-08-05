using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsCost
{
    public class UpdateVehicleCostRequest : AuditableRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public VehicleCostType Type { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
