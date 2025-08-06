using BusinessLogic.Common;
using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IVehicleCostRepository : IRepository<VehicleCost>
    {
        Task<List<VehicleCost>> GetVehicleCostIdAsync(QueryOptions options,int vehicleId);
    }
}
