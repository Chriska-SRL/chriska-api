
using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
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
            var subcategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                ?? throw new ArgumentException("La subcategoría especificada no existe.", nameof(request.SubCategoryId));
            var newProduct = ProductMapper.ToDomain(request, subcategory);
            newProduct.Validate();

            var existing = await _productRepository.GetByBarcodeAsync(newProduct.InternalCode);
            if (existing != null)
                throw new ArgumentException("Ya existe un producto con ese código interno.", nameof(newProduct.InternalCode));

            var subCategory = await _subCategoryRepository.GetByIdAsync(newProduct.SubCategory.Id);
            if (subCategory == null)
                throw new ArgumentException("La subcategoría especificada no existe.", nameof(newProduct.SubCategory.Id));

            var added = await _productRepository.AddAsync(newProduct);
            return ProductMapper.ToResponse(added);
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request)
        {
            var existingProduct = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.", nameof(request.Id));

            var otherWithSameCode = await _productRepository.GetByBarcodeAsync(request.Barcode);
            if (existingProduct.InternalCode != request.Barcode && otherWithSameCode != null)
                throw new ArgumentException("Ya existe otro producto con ese código interno.", nameof(request.Barcode));

            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId);
            if (subCategory == null)
                throw new ArgumentException("La subcategoría especificada no existe.", nameof(request.SubCategoryId));

            var updatedData = ProductMapper.ToUpdatableData(request, subCategory);
            existingProduct.Update(updatedData);

            var updated = await _productRepository.UpdateAsync(existingProduct);
            return ProductMapper.ToResponse(updated);
        }
        public async Task DeleteProductAsync(DeleteRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.", nameof(request.Id));

            product.MarkAsDeleted(request.getUserId(), request.Location);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.", nameof(id));

            return ProductMapper.ToResponse(product);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(QueryOptions options)
        {
            var products = await _productRepository.GetAllAsync(options);
            return products.Select(ProductMapper.ToResponse).ToList();
        }
    }
}
