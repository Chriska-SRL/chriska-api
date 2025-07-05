using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.DTOs.DTOsVehicle
{
    public class VehicleResponse
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public List<VehicleCostResponse> Costs { get; set; }


        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
