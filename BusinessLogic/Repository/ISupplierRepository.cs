using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier> GetByNameAsync(string name);
        Task<Supplier> GetByRUTAsync(string rUT);
    }
}
