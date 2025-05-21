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
        // Guía temporal: entidades que maneja este subsistema

        private List<Product> Products = new List<Product>();
        
        private IProductRepository _productRepository;
        public ProductsSubSystem(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
    }
}
