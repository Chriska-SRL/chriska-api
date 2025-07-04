using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsBrand;

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

        public ProductResponse AddProduct(AddProductRequest request)
        {
            var subCategory = _subCategoryRepository.GetById(request.SubCategoryId)
                              ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.SubCategoryId));
            var brand = _brandRepository.GetById(request.BrandId)
                              ?? throw new ArgumentException("Marca no encontrada.", nameof(request.BrandId));

            if ( _productRepository.GetByBarcode(request.Barcode) != null) 
                            throw new ArgumentException("Ya existe un producto con el mismo código de barras.", nameof(request.Barcode));

            if(_productRepository.GetByName(request.Name) != null) 
                            throw new ArgumentException("Ya existe un producto con el mismo nombre.", nameof(request.Name));

            var newProduct = ProductMapper.ToDomain(request, subCategory);
            newProduct.Brand = brand;
            newProduct.Validate();

            var added = _productRepository.Add(newProduct);
            added.SetInternalCode();
            return ProductMapper.ToResponse(added);
        }

        public ProductResponse UpdateProduct(UpdateProductRequest request)
        {
            var existing = _productRepository.GetById(request.Id)
                           ?? throw new ArgumentException("Producto no encontrado.", nameof(request.Id));

            var subCategory = _subCategoryRepository.GetById(request.SubCategoryId)
                              ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.SubCategoryId));

            var brand = _brandRepository.GetById(request.BrandId)
                           ?? throw new ArgumentException("Marca no encontrada.", nameof(request.BrandId));

            if (existing.Barcode != request.Barcode && _productRepository.GetByBarcode(request.Barcode) != null)
                throw new ArgumentException("Ya existe un producto con el mismo código de barras.", nameof(request.Barcode));

            if (existing.Name != request.Name &&  _productRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un producto con el mismo nombre.", nameof(request.Name));

            Product.UpdatableData data = ProductMapper.ToUpdatableData(request, subCategory);
            data.Brand = brand;
            existing.Update(data);

            Product updated = _productRepository.Update(existing);
            updated.SetInternalCode();
            return ProductMapper.ToResponse(updated);
        }

        public ProductResponse DeleteProduct(int id)
        {
            var deleted = _productRepository.GetById(id)
                          ?? throw new ArgumentException("Producto no encontrado.", nameof(id));


            //TODO: Implementar control de integridad referencial:
            //cuado se trabaje con entidades que tengan relacion con esta.
            deleted = _productRepository.Delete(id);
            deleted.SetInternalCode();
            return ProductMapper.ToResponse(deleted);
        }

        public ProductResponse GetProductById(int id)
        {
            var product = _productRepository.GetById(id)
                          ?? throw new ArgumentException("Producto no encontrado.", nameof(id));
            product.SetInternalCode();
            return ProductMapper.ToResponse(product);
        }

        public List<ProductResponse> GetAllProducts()
        {
            return _productRepository.GetAll()
                                     .Select(p =>
                                     {
                                         p.SetInternalCode();
                                         return ProductMapper.ToResponse(p);
                                     })
                                     .ToList();
        }

        public BrandResponse AddBrand(AddBrandRequest request)
        {
            var newBrand = BrandMapper.ToDomain(request);
            newBrand.Validate();

            var added = _brandRepository.Add(newBrand);
            return BrandMapper.ToResponse(added);
        }

        public BrandResponse UpdateBrand(UpdateBrandRequest request)
        {
            var existing = _brandRepository.GetById(request.Id)
                           ?? throw new ArgumentException("Marca no encontrada.", nameof(request.Id));

            Brand.UpdatableData data = BrandMapper.ToUpdatableData(request);
            existing.Update(data);

            var updated = _brandRepository.Update(existing);
            return BrandMapper.ToResponse(updated);
        }

        public BrandResponse DeleteBrand(int id)
        {
            var brand = _brandRepository.GetById(id)
                        ?? throw new ArgumentException("Marca no encontrada.", nameof(id));

            var deleted = _brandRepository.Delete(id);
            return BrandMapper.ToResponse(deleted);
        }

        public BrandResponse GetBrandById(int id)
        {
            var brand = _brandRepository.GetById(id)
                        ?? throw new ArgumentException("Marca no encontrada.", nameof(id));

            return BrandMapper.ToResponse(brand);
        }

        public List<BrandResponse> GetAllBrands()
        {
            return _brandRepository.GetAll()
                                   .Select(BrandMapper.ToResponse)
                                   .ToList();
        }
    }
}
