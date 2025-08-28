using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Services;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IShelveRepository _shelveRepository;
        private readonly IAzureBlobService _blobService;

        public ProductsSubSystem(
            IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,
            IBrandRepository brandRepository,
            ISupplierRepository supplierRepository,
            IShelveRepository shelveRepository,
            IAzureBlobService blobService)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _brandRepository = brandRepository;
            _supplierRepository = supplierRepository;
            _shelveRepository = shelveRepository;
            _blobService = blobService;
        }

        public async Task<ProductResponse> AddProductAsync(ProductAddRequest request)
        {
            SubCategory subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                ?? throw new ArgumentException("No se encontró la subcategoría asociada.");

            Brand brand = await _brandRepository.GetByIdAsync(request.BrandId)
                ?? throw new ArgumentException("No se encontró la marca asociada.");

            Shelve shelve = await _shelveRepository.GetByIdAsync(request.ShelveId)
                ?? throw new ArgumentException("No se encontró la estantería asociada.");

            List<Supplier> suppliers = new List<Supplier>();
            foreach (var supplierId in request.SupplierIds)
            {
                var supplier = await _supplierRepository.GetByIdAsync(supplierId)
                    ?? throw new ArgumentException($"No se encontró el proveedor con ID {supplierId}.");
                suppliers.Add(supplier);
            }

            var product = ProductMapper.ToDomain(request, subCategory, brand, suppliers, shelve);
            product.Validate();

            var added = await _productRepository.AddAsync(product);
            return ProductMapper.ToResponse(added);
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request)
        {
            var existing = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                ?? throw new ArgumentException("No se encontró la subcategoría asociada.");

            var brand = await _brandRepository.GetByIdAsync(request.BrandId)
                ?? throw new ArgumentException("No se encontró la marca asociada.");

            var shelve = await _shelveRepository.GetByIdAsync(request.ShelveId)
                ?? throw new ArgumentException("No se encontró la estantería asociada.");

            List<Supplier> suppliers = new List<Supplier>();
            foreach (var supplierId in request.SupplierIds)
            {
                var supplier = await _supplierRepository.GetByIdAsync(supplierId)
                    ?? throw new ArgumentException($"No se encontró el proveedor con ID {supplierId}.");
                suppliers.Add(supplier);
            }

            Product.UpdatableData updatedData = ProductMapper.ToUpdatableData(request, subCategory, brand, suppliers, shelve);
            existing.Update(updatedData);

            var updated = await _productRepository.UpdateAsync(existing);
            return ProductMapper.ToResponse(updated);
        }

        public async Task DeleteProductAsync(DeleteRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            product.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            return ProductMapper.ToResponse(product);
        }

        public async Task<ProductResponse> GetProductByIdWithDiscountsAsync(int id)
        {
            var product = await _productRepository.GetByIdWithDiscountsAsync(id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            return ProductMapper.ToResponse(product);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(QueryOptions options)
        {
            var products = await _productRepository.GetAllAsync(options);
            return products.Select(ProductMapper.ToResponse).ToList();
        }

        public async Task<string> UploadProductImageAsync(AddImageRequest request)
        {
            Product product = await _productRepository.GetByIdAsync(request.EntityId)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");
            product.AuditInfo.SetUpdated(request.getUserId(), request.AuditLocation);

            var url = await _blobService.UploadFileAsync(request.File, "products", $"product{product.Id}");
            return await _productRepository.UpdateImageUrlAsync(product, url);
        }

        public async Task DeleteProductImageAsync(int productId)
        {
            Product product = await _productRepository.GetByIdAsync(productId)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            await _blobService.DeleteFileAsync(product.ImageUrl, "products"); 
            await _productRepository.UpdateImageUrlAsync(product, "");
        }
    }
}
