using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(AddProductRequest dto, SubCategory subCategory)
        {
            return new Product(
                id: 0,
                barcode: dto.Barcode,
                name: dto.Name,
                price: dto.Price,
                image: dto.Image,
                stock: 0,
                description: dto.Description,
                unitType: dto.UnitType,
                temperatureCondition: dto.TemperatureCondition,
                observation: dto.Observation,
                subCategory: subCategory,
                suppliers: new List<Supplier>()
            );
        }

        public static Product.UpdatableData ToUpdatableData(UpdateProductRequest dto, SubCategory subCategory)
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
                Observation = dto.Observation,
                SubCategory = subCategory
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
                Description = domain.Description,
                UnitType = domain.UnitType,
                TemperatureCondition = domain.TemperatureCondition,
                Observation = domain.Observation,
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
                Suppliers = domain.Suppliers.Select(s => new SupplierResponse
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            };
        }
    }
}
