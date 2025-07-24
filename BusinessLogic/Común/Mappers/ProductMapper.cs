using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsDiscount;
using BusinessLogic.Domain;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductMapper
    {
        public static Product FromAddRequest(AddProductRequest dto, SubCategory subCategory)
        {
            return new Product(
                id: 0,
                barcode: dto.Barcode,
                name: dto.Name,
                price: dto.Price,
                image: dto.Image,
                stock: dto.Stock,
                aviableStock: dto.AvailableStock,
                description: dto.Description,
                unitType: dto.UnitType,
                temperatureCondition: dto.TemperatureCondition,
                observations: dto.Observations,
                subCategory: subCategory,
                brand: new Brand(dto.BrandId),
                suppliers: new List<Supplier>(), 
                auditInfo: AuditMapper.ToDomain(dto.AuditInfo)
            );
        }

        public static Product.UpdatableData FromUpdateRequest(UpdateProductRequest dto, SubCategory subCategory)
        {
            return new Product.UpdatableData
            {
                Name = dto.Name,
                Barcode = dto.Barcode,
                Price = dto.Price,
                Image = dto.Image,
                Description = dto.Description,
                UnitType = dto.UnitType,
                TemperatureCondition = dto.TemperatureCondition,
                Observation = dto.Observations,
                SubCategory = subCategory,
                Brand = new Brand(dto.BrandId),
                Stock = dto.Stock,
                AviableStock = dto.AvailableStock,
                AuditInfo = AuditMapper.ToDomain(dto.AuditInfo),
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
                Image = product.Image,
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
                Discounts = product.Discounts.Select(d => new DiscountResponse
                {
                    Description = d.Description,
                    ExpirationDate = d.ExpirationDate,
                    Percentage = d.Percentage,
                    ProductQuantity = d.ProductQuantity
                }).ToList(),
                AuditInfo = AuditMapper.ToResponse(product.AuditInfo)
            };
        }
    }
}
