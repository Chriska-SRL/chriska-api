using BusinessLogic.Común.Enums;

namespace BusinessLogic.DTOs.DTOsCost
{
    public class AddVehicleCostRequest
    {
        public int VehicleId { get; set; }
        public DateTime date { get; set; }
        public VehicleCostType Type { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
