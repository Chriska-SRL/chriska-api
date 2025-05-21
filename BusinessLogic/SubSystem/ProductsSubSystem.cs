using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private IProductRepository _productRepository;
        private ISubCategoryRepository _subCategoryRepository;
        public ProductsSubSystem(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
        }
        public void AddProduct(Product product)
        {
            _productRepository.Add(product);
        }
        public void AddSubCategory(SubCategory subCategory)
        {
            _subCategoryRepository.Add(subCategory);
        }
    }
}
