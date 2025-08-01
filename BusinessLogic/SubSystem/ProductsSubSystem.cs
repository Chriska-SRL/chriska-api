using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IAzureBlobService _blobService;

        public ProductsSubSystem(
            IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,
            IBrandRepository brandRepository,
            IAzureBlobService blobService)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _brandRepository = brandRepository;
            _blobService = blobService;
        }

        public async Task<ProductResponse> AddProductAsync(ProductAddRequest request)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                ?? throw new ArgumentException("No se encontró la subcategoría asociada.", nameof(request.SubCategoryId));

            var brand = await _brandRepository.GetByIdAsync(request.BrandId)
                ?? throw new ArgumentException("No se encontró la marca asociada.", nameof(request.BrandId));

            var product = ProductMapper.ToDomain(request);
            product.Validate();

            var added = await _productRepository.AddAsync(product);
            added.SubCategory = subCategory;
            added.Brand = brand;
            return ProductMapper.ToResponse(added);
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request)
        {
            var existing = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.", nameof(request.Id));

            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                ?? throw new ArgumentException("No se encontró la subcategoría asociada.", nameof(request.SubCategoryId));

            var brand = await _brandRepository.GetByIdAsync(request.BrandId)
                ?? throw new ArgumentException("No se encontró la marca asociada.", nameof(request.BrandId));

            var updatedData = ProductMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _productRepository.UpdateAsync(existing);
            updated.SubCategory = subCategory;
            updated.Brand = brand;
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

        public async Task UploadImageAsync(int productId, IFormFile file, int userId)
        {
            var url = await _blobService.UploadFileAsync(file, "products");
            await _productRepository.UpdateImageUrlAsync(productId, url, userId);
        }
    }
}
