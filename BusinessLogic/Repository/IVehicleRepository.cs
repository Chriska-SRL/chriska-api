using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle?> GetByPlateAsync(string plate);
    }
}
