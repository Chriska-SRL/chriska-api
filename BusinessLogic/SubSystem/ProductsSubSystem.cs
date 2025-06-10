using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Común.Mappers;

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

        public ProductResponse AddProduct(AddProductRequest request)
        {
            var subCategory = _subCategoryRepository.GetById(request.SubCategoryId)
                              ?? throw new InvalidOperationException("Subcategoría no encontrada.");

            var newProduct = ProductMapper.ToDomain(request, subCategory);
            newProduct.Validate();

            var added = _productRepository.Add(newProduct);
            return ProductMapper.ToResponse(added);
        }

        public ProductResponse UpdateProduct(UpdateProductRequest request)
        {
            var existing = _productRepository.GetById(request.Id)
                           ?? throw new InvalidOperationException("Producto no encontrado.");

            var subCategory = _subCategoryRepository.GetById(request.SubCategoryId)
                              ?? throw new InvalidOperationException("Subcategoría no encontrada.");

            var data = ProductMapper.ToUpdatableData(request, subCategory);
            existing.Update(data);

            var updated = _productRepository.Update(existing);
            return ProductMapper.ToResponse(updated);
        }

        public ProductResponse DeleteProduct(DeleteProductRequest request)
        {
            var deleted = _productRepository.Delete(request.Id)
                          ?? throw new InvalidOperationException("Producto no encontrado.");

            return ProductMapper.ToResponse(deleted);
        }

        public ProductResponse GetProductById(int id)
        {
            var product = _productRepository.GetById(id)
                          ?? throw new InvalidOperationException("Producto no encontrado.");

            return ProductMapper.ToResponse(product);
        }

        public List<ProductResponse> GetAllProducts()
        {
            return _productRepository.GetAll()
                                     .Select(ProductMapper.ToResponse)
                                     .ToList();
        }
    }
}
