using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByBarcodeAsync(string? barcode);
    }
}
