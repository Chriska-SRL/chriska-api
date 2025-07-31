using BusinessLogic.Común;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public ProductsSubSystem(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<ProductResponse> AddProductAsync(ProductAddRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}