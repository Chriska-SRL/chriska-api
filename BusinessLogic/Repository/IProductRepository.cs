using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<string> UpdateImageUrlAsync(Product product, string imageUrl);
    }
}
