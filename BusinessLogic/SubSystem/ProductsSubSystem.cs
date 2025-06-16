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
                              ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.SubCategoryId));

            if( _productRepository.GetByBarcode(request.Barcode) != null) 
                            throw new ArgumentException("Ya existe un producto con el mismo código de barras.", nameof(request.Barcode));

            if(_productRepository.GetByName(request.Name) != null) 
                            throw new ArgumentException("Ya existe un producto con el mismo nombre.", nameof(request.Name));

            var newProduct = ProductMapper.ToDomain(request, subCategory);
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

            if (existing.Barcode != request.Barcode && _productRepository.GetByBarcode(request.Barcode) != null)
                throw new ArgumentException("Ya existe un producto con el mismo código de barras.", nameof(request.Barcode));

            if (existing.Name != request.Name &&  _productRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un producto con el mismo nombre.", nameof(request.Name));

            Product.UpdatableData data = ProductMapper.ToUpdatableData(request, subCategory);
            existing.Update(data);

            Product updated = _productRepository.Update(existing);
            return ProductMapper.ToResponse(updated);
        }

        public ProductResponse DeleteProduct(int id)
        {
            var deleted = _productRepository.Delete(id)
                          ?? throw new ArgumentException("Producto no encontrado.", nameof(id));

            //TODO: Implementar control de integridad referencial:

            return ProductMapper.ToResponse(deleted);
        }

        public ProductResponse GetProductById(int id)
        {
            var product = _productRepository.GetById(id)
                          ?? throw new ArgumentException("Producto no encontrado.", nameof(id));

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
