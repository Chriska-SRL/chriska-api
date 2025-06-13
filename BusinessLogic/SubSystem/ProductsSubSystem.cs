using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;

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

            Product newProduct = ProductMapper.ToDomain(request, subCategory);
            newProduct.Validate();

            Product added = _productRepository.Add(newProduct);
            return ProductMapper.ToResponse(added);
        }

        public ProductResponse UpdateProduct(UpdateProductRequest request)
        {
            Product existing = _productRepository.GetById(request.Id)
                                ?? throw new InvalidOperationException("Producto no encontrado.");

            SubCategory subCategory = _subCategoryRepository.GetById(request.SubCategoryId)
                              ?? throw new InvalidOperationException("Subcategoría no encontrada.");

            Product.UpdatableData data = ProductMapper.ToUpdatableData(request, subCategory);
            existing.Update(data);

            Product updated = _productRepository.Update(existing);
            return ProductMapper.ToResponse(updated);
        }

        public ProductResponse DeleteProduct(DeleteProductRequest request)
        {
            Product deleted = _productRepository.Delete(request.Id)
                                ?? throw new InvalidOperationException("Producto no encontrado.");

            return ProductMapper.ToResponse(deleted);
        }

        public ProductResponse GetProductById(int id)
        {
            Product product = _productRepository.GetById(id)
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
