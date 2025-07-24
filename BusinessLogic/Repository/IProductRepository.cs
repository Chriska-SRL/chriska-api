using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByBarcodeAsync(object barcode);
        Task<Product> GetByNameAsync(object name);
        Task<List<Product>> GetBySubCategoryId(int id);
    }
}
