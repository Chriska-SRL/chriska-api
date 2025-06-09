using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;

        public ProductsSubSystem(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(AddProductRequest product)
        {
            Product newProduct = ProductMapper.toDomain(product);
            newProduct.Validate();
            _productRepository.Add(newProduct);
        }

        public void UpdateProduct(UpdateProductRequest product)
        {
            Product existingProduct = _productRepository.GetById(product.Id);
            if (existingProduct == null) throw new Exception("No se encontro el producto");
            existingProduct.Update(ProductMapper.toDomain(product));
            _productRepository.Update(existingProduct);
        }

        public void DeleteProduct(DeleteProductRequest deleteProductRequest)
        {
            Product existingProduct = _productRepository.GetById(deleteProductRequest.Id);
            if (existingProduct == null) throw new Exception("No se encontro el producto");
            _productRepository.Delete(deleteProductRequest.Id);
        }

        public ProductResponse GetProductById(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product == null) throw new Exception("No se encontro el producto");
            ProductResponse productResponse = ProductMapper.toResponse(product);
            return productResponse;
        }

        public List<ProductResponse> GetAllProducts()
        {
            List<Product> products = _productRepository.GetAll();
            if (products == null || products.Count == 0) throw new Exception("No se encontraron productos");
            return products.Select(ProductMapper.toResponse).ToList(); ;
        }
    } }
