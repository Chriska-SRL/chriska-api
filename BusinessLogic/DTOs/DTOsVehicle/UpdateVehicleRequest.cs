using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsVehicle
{
    public class UpdateVehicleRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
    }
}
