using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IVehicleCostRepository : IRepository<VehicleCost>
    {
        Task<List<VehicleCost>> GetAllForVehicleAsync(int id);
        Task<IEnumerable<VehicleCost>> GetCostsForVehicleInDateRangeAsync(int vehicleId, DateTime from, DateTime to);
    }
}
