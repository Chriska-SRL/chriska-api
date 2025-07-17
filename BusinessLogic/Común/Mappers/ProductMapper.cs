using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;
using BusinessLogic.Común.Audits;
using BusinessLogic.DTOs.DTOsDiscount;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(ProductAddRequest dto, SubCategory subCategory)
        {
            var product = new Product(
                id: 0,
                barcode: dto.Barcode,
                name: dto.Name,
                price: dto.Price,
                image: dto.Image,
                stock: dto.Stock,
                description: dto.Description,
                unitType: dto.UnitType,
                temperatureCondition: dto.TemperatureCondition,
                observations: dto.Observations,
                subCategory: subCategory,
                brand: new Brand(dto.BrandId)
            )
            {
                AviableStock = dto.AvailableStock
            };

            if (dto.AuditInfo?.Created != null)
            {
                product.CreatedAt = dto.AuditInfo.Created.At;
                product.CreatedBy = dto.AuditInfo.Created.By?.Username;
                product.CreatedLocation = dto.AuditInfo.Created.Location;
            }

            return product;
        }

        public static Product.UpdatableData ToUpdatableData(ProductUpdateRequest dto, SubCategory subCategory)
        {
            return new Product.UpdatableData
            {
                Name = dto.Name,
                Price = dto.Price,
                Barcode = dto.Barcode,
                Image = dto.Image,
                Description = dto.Description,
                UnitType = dto.UnitType,
                TemperatureCondition = dto.TemperatureCondition,
                Observation = dto.Observations,
                SubCategory = subCategory,
                Brand = new Brand(dto.BrandId),
                Stock = dto.Stock,
                AviableStock = dto.AvailableStock,
                AuditInfo = dto.AuditInfo
            };
        }

        public static ProductResponse ToResponse(Product domain)
        {
            return new ProductResponse
            {
                Id = domain.Id,
                InternalCode = domain.InternalCode,
                Barcode = domain.Barcode,
                Name = domain.Name,
                Price = domain.Price,
                Image = domain.Image,
                Stock = domain.Stock,
                AvailableStock = domain.AviableStock,
                Description = domain.Description,
                UnitType = domain.UnitType,
                TemperatureCondition = domain.TemperatureCondition,
                Observations = domain.Observation,
                SubCategory = new SubCategoryResponse
                {
                    Id = domain.SubCategory.Id,
                    Name = domain.SubCategory.Name,
                    Description = domain.SubCategory.Description,
                    Category = new CategoryResponse
                    {
                        Id = domain.SubCategory.Category.Id,
                        Name = domain.SubCategory.Category.Name,
                        Description = domain.SubCategory.Category.Description
                    }
                },
                Brand = new BrandResponse
                {
                    Id = domain.Brand.Id,
                    Name = domain.Brand.Name,
                    Description = domain.Brand.Description
                },
                Discounts = domain.Discounts.Select(d => new DiscountResponse
                {
                    Description = d.Description,
                    ExpirationDate = d.ExpirationDate,
                    Percentage = d.Percentage,
                    ProductQuantity = d.ProductQuantity
                }).ToList(),
                AuditInfo = new AuditInfoResponse
                {
                    Created = domain.CreatedAt != default ? new AuditAction
                    {
                        At = domain.CreatedAt,
                        By = new AuditUser { Username = domain.CreatedBy ?? string.Empty },
                        Location = domain.CreatedLocation ?? string.Empty
                    } : null,
                    Updated = domain.UpdatedAt.HasValue ? new AuditAction
                    {
                        At = domain.UpdatedAt.Value,
                        By = new AuditUser { Username = domain.UpdatedBy ?? string.Empty },
                        Location = domain.UpdatedLocation ?? string.Empty
                    } : null,
                    Deleted = domain.DeletedAt.HasValue ? new AuditAction
                    {
                        At = domain.DeletedAt.Value,
                        By = new AuditUser { Username = domain.DeletedBy ?? string.Empty },
                        Location = domain.DeletedLocation ?? string.Empty
                    } : null
                }
            };
        }
    }
}
