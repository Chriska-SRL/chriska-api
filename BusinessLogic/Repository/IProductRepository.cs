using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByIdWithDiscountsAsync(int productId);
        Task<string> UpdateImageUrlAsync(Product product, string imageUrl);
        Task UpdateStockAsync(int productId, int stock,int availableStock);
    }
}
