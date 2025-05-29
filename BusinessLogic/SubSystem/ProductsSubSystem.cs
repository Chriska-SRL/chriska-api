using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsProduct;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        private readonly CategoriesSubSystem _categoriesSubSystem;

        public ProductsSubSystem(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository, CategoriesSubSystem categoriesSubSystem)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;

            _categoriesSubSystem = categoriesSubSystem;
        }

        public void AddProduct(AddProductRequest product)
        {
            var newProduct = new Product(product.InternalCode, product.Barcode, product.Name, product.Price, product.Image, product.Stock, product.Description, product.UnitType, product.TemperatureCondition, product.Observation, _subCategoryRepository.GetById(product.SubCategoryId));
            newProduct.Validate();
        }

        public void UpdateProduct(UpdateProductRequest product)
        {
            var existingProduct = _productRepository.GetById(product.Id);
            if (existingProduct == null)
                throw new Exception("No se encontro el producto");
            existingProduct.Update(product.Name, product.Price, product.Image, product.Stock, product.Description, product.UnitType, product.TemperatureCondition, product.Observation, _subCategoryRepository.GetById(product.Id));
            _productRepository.Update(existingProduct);
        }

        public void DeleteProduct(DeleteProductRequest deleteProductRequest)
        {
            var existingProduct = _productRepository.GetById(deleteProductRequest.Id);
            if (existingProduct == null) throw new Exception("No se encontro el producto");
            _productRepository.Delete(deleteProductRequest.Id);
        }

        public ProductResponse GetProductById(int id)
        {
            var product = _productRepository.GetById(id);
            return new ProductResponse
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                SubCategory = _categoriesSubSystem.GetSubCategoryById(product.SubCategory.Id)
            };
        }

        public List<ProductResponse> GetAllProducts()
        {
            var products = _productRepository.GetAll();
            return products.Select(p => new ProductResponse
            {
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Description = p.Description,
                SubCategory = _categoriesSubSystem.GetSubCategoryById(p.SubCategory.Id),
            }).ToList();
        }
    } }
