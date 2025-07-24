using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle> GetByPlateAsync(string plate);
    }
}
