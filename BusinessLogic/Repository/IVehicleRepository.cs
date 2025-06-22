using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IVehicleRepository:IRepository<Vehicle>
    {
        Vehicle GetByPlate(string plate);
    }
}
