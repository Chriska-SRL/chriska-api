using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Task<Warehouse> GetByNameAsync(string name);
    }
}
