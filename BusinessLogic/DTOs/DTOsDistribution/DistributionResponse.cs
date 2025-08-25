using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.DTOs.DTOsZone;


namespace BusinessLogic.DTOs.DTOsDistribution
{
    public class DistributionResponse
    {
        public int Id { get; set; }
        public string? Observations { get; set; }
        public UserResponse? User { get; set; }
        public VehicleResponse? Vehicle { get; set; }
        public List<ZoneResponse>? Zones { get; set; }
        public List<DeliveryResponse>? Deliveries { get; set; }
    }
}
