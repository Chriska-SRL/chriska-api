using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.DTOs.DTOsVehicle
{
    public class VehicleResponse : AuditableResponse
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public List<VehicleCostResponse> Costs { get; set; }
    }
}
