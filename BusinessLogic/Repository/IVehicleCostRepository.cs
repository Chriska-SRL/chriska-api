using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IVehicleCostRepository : IRepository<VehicleCost>
    {
        Task<List<VehicleCost>> GetByVehicleIdAndDateRangeAsync(int vehicleId, DateTime from, DateTime to);
        Task<List<VehicleCost>> GetVehicleCostIdAsync(int vehicleId);
    }
}
