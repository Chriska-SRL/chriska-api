using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier?> GetByNameAsync(string name);
        Task<Supplier?> GetByRazonSocialAsync(string razonSocial);
        Task<Supplier?> GetByRUTAsync(string rut);
    }
}
