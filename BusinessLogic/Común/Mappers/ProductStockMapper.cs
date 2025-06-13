using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsProductStock;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductStockMapper
    {
        public static ProductStock ToDomain(AddProductStockRequest product)
        {
            return new ProductStock
            (
                id: 0,
                quantity: product.Quantity,
                product: new Product(product.ProductId)
            );
        }

        public static ProductStock.UpdatableData ToDomain(UpdateProductStockRequest product)
        {
            return new ProductStock.UpdatableData
            {
                Quantity = product.Quantity,
                Product = new Product(product.ProductId)
            };
        }

        public static ProductStockResponse ToResponse(ProductStock productStock)
        {
            return new ProductStockResponse
            {
                Quantity = productStock.Quantity,
                Product = new ProductResponse
                {
                    Id = productStock.Product.Id,
                    InternalCode = productStock.Product.InternalCode,
                    Barcode = productStock.Product.Barcode,
                    Name = productStock.Product.Name,
                    Price = productStock.Product.Price,
                    Image = productStock.Product.Image,
                    Stock = productStock.Product.Stock,
                    Description = productStock.Product.Description,
                    UnitType = productStock.Product.UnitType,
                    TemperatureCondition = productStock.Product.TemperatureCondition,
                    Observation = productStock.Product.Observation,
                    SubCategory = new SubCategoryResponse
                    {
                        Id = productStock.Product.SubCategory.Id,
                        Name = productStock.Product.SubCategory.Name
                    }
                }
            };
        }
    }
}
