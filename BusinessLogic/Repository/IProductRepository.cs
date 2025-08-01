using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetBySubCategoryIdAsync(int id);
    }
}
