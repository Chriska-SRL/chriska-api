using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsVehicle;

namespace BusinessLogic.DTOs.DTOsCost
{
    public class VehicleCostResponse : AuditableResponse
    {
        public VehicleResponse Vehicle { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public VehicleCostType Type { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
