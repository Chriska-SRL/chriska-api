using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetByBarcode(string barcode);
        Product GetByName(string name);
        List<Product> GetBySubCategoryId(int id);
    }
}
