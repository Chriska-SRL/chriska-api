using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsBrand;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(ProductAddRequest request, SubCategory subCategory)
        {
            Product product = new Product(
                barcode: request.Barcode,
                name: request.Name,
                price: request.Price,
                image: request.Image,
                stock: request.Stock,
                aviableStock: request.AvailableStock,
                description: request.Description,
                unitType: request.UnitType,
                temperatureCondition: request.TemperatureCondition,
                observations: request.Observations,
                subCategory: subCategory,
                brand: new Brand(request.BrandId),
                suppliers: new List<Supplier>()
            );

            product.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return product;
        }

        public static Product.UpdatableData ToUpdatableData(ProductUpdateRequest request, SubCategory subCategory)
        {
            return new Product.UpdatableData
            {
                Name = request.Name,
                Barcode = request.Barcode,
                Price = request.Price,
                Image = request.Image,
                Description = request.Description,
                UnitType = request.UnitType,
                TemperatureCondition = request.TemperatureCondition,
                Observation = request.Observations,
                SubCategory = subCategory,
                Brand = new Brand(request.BrandId),
                Stock = request.Stock,
                AviableStock = request.AvailableStock,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static ProductResponse ToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                InternalCode = product.InternalCode,
                Barcode = product.Barcode,
                Name = product.Name,
                Price = product.Price,
                Image = product.ImageUrl,
                Stock = product.Stock,
                AvailableStock = product.AviableStock,
                Description = product.Description,
                UnitType = product.UnitType,
                TemperatureCondition = product.TemperatureCondition,
                Observations = product.Observation,
                SubCategory = new SubCategoryResponse
                {
                    Id = product.SubCategory.Id,
                    Name = product.SubCategory.Name,
                    Description = product.SubCategory.Description,
                    Category = new CategoryResponse
                    {
                        Id = product.SubCategory.Category.Id,
                        Name = product.SubCategory.Category.Name,
                        Description = product.SubCategory.Category.Description
                    }
                },
                Brand = new BrandResponse
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name,
                    Description = product.Brand.Description
                },
                AuditInfo = AuditMapper.ToResponse(product.AuditInfo)
            };
        }
    }
}
