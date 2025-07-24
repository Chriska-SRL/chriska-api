using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.Común;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductsSubSystem(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _brandRepository = brandRepository;
        }

        public async Task<ProductResponse> AddProduct(AddProductRequest request)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                              ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.SubCategoryId));

            var brand = await _brandRepository.GetByIdAsync(request.BrandId)
                        ?? throw new ArgumentException("Marca no encontrada.", nameof(request.BrandId));

            if (await _productRepository.GetByBarcodeAsync(request.Barcode) != null)
                throw new ArgumentException("Ya existe un producto con el mismo código de barras.", nameof(request.Barcode));

            if (await _productRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un producto con el mismo nombre.", nameof(request.Name));

            var newProduct = ProductMapper.FromAddRequest(request, subCategory);
            newProduct.Brand = brand;
            newProduct.Validate();

            var added = await _productRepository.AddAsync(newProduct);
            added.SetInternalCode();

            return ProductMapper.ToResponse(added);
        }

        public async Task<ProductResponse> UpdateProduct(UpdateProductRequest request)
        {
            var existing = await _productRepository.GetByIdAsync(request.Id)
                           ?? throw new ArgumentException("Producto no encontrado.", nameof(request.Id));

            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                                ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.SubCategoryId));

            var brand = await _brandRepository.GetByIdAsync(request.BrandId)
                         ?? throw new ArgumentException("Marca no encontrada.", nameof(request.BrandId));

            if (existing.Barcode != request.Barcode && await _productRepository.GetByBarcodeAsync(request.Barcode) != null)
                throw new ArgumentException("Ya existe un producto con el mismo código de barras.", nameof(request.Barcode));

            if (existing.Name != request.Name && await _productRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un producto con el mismo nombre.", nameof(request.Name));

            var data = ProductMapper.FromUpdateRequest(request, subCategory);
            data.Brand = brand;

            existing.Update(data);

            var updated = await _productRepository.UpdateAsync(existing);
            updated.SetInternalCode();

            return ProductMapper.ToResponse(updated);
        }

        public async Task<ProductResponse> DeleteProduct(DeleteProductRequest request)
        {
            var deleted = await _productRepository.GetByIdAsync(request.Id)
                          ?? throw new ArgumentException("Producto no encontrado.", nameof(request.Id));

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            deleted.SetDeletedAudit(auditInfo);

            await _productRepository.DeleteAsync(deleted);
            deleted.SetInternalCode();

            return ProductMapper.ToResponse(deleted);
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                          ?? throw new ArgumentException("Producto no encontrado.", nameof(id));

            product.SetInternalCode();
            return ProductMapper.ToResponse(product);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(QueryOptions options)
        {
            var products = await _productRepository.GetAllAsync(options);
            return products.Select(p =>
            {
                p.SetInternalCode();
                return ProductMapper.ToResponse(p);
            }).ToList();
        }


        public async Task<BrandResponse> AddBrandAsync(AddBrandRequest request)
        {
            if (await _brandRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una marca con el mismo nombre.", nameof(request.Name));

            var newBrand = BrandMapper.ToDomain(request);
            newBrand.Validate();

            var added = await _brandRepository.AddAsync(newBrand);
            return BrandMapper.ToResponse(added);
        }

        public async Task<BrandResponse> UpdateBrandAsync(UpdateBrandRequest request)
        {
            var existing = await _brandRepository.GetByIdAsync(request.Id)
                            ?? throw new ArgumentException("Marca no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && await _brandRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una marca con el mismo nombre.", nameof(request.Name));

            var data = BrandMapper.ToUpdatableData(request);
            existing.Update(data);

            var updated = await _brandRepository.UpdateAsync(existing);
            return BrandMapper.ToResponse(updated);
        }

          public async Task<BrandResponse> DeleteBrandAsync(DeleteBrandRequest request)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id)
                         ?? throw new ArgumentException("Marca no encontrada.", nameof(request.Id));

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            brand.SetDeletedAudit(auditInfo);

            await _brandRepository.DeleteAsync(brand);
            return BrandMapper.ToResponse(brand);
        }

        public async Task<BrandResponse> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id)
                         ?? throw new ArgumentException("Marca no encontrada.", nameof(id));

            return BrandMapper.ToResponse(brand);
        }

        public async Task<List<BrandResponse>> GetAllBrandsAsync(QueryOptions options)
        {
            var brands = await _brandRepository.GetAllAsync(options);
            return brands.Select(BrandMapper.ToResponse).ToList();
        }

    }
}
