using BusinessLogic.Común.Enums;

namespace BusinessLogic.DTOs.DTOsCost
{
    public class VehicleCostResponse
    {
        public int VehicleId { get; set; }
        public int Id { get; set; }
        public DateTime date { get; set; }
        public VehicleCostType CostType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
