using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand?> GetByNameAsync(string name);

        Task<Brand?> GetByIdWithProductsAsync(int id);
    }
}
