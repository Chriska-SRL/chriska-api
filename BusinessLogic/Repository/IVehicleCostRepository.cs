using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IVehicleCostRepository: IRepository<VehicleCost>
    {
        List<VehicleCost> GetAllForVehicle(int vehicleId);
        List<VehicleCost> GetCostsForVehicleInDateRange(int vehicleId, DateTime from, DateTime to);
    }
}
