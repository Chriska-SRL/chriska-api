using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDiscount;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Common.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(ProductAddRequest request, SubCategory subCategory, Brand brand, List<Supplier> suppliers, Shelve shelve)
        {
            Product product = new Product(
                barcode: request.Barcode,
                name: request.Name,
                price: request.Price,
                description: request.Description,
                unitType: request.UnitType,
                temperatureCondition: request.TemperatureCondition,
                estimatedWeight: request.EstimatedWeight ?? 0,
                observations: request.Observations ?? "",
                subCategory: subCategory,
                brand: brand,
                suppliers: suppliers,
                shelve: shelve
            );

            product.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);
            return product;
        }

        public static Product.UpdatableData ToUpdatableData(ProductUpdateRequest request, SubCategory subCategory, Brand brand, List<Supplier> suppliers, Shelve shelve)
        {
            return new Product.UpdatableData
            {
                Name = request.Name,
                Barcode = request.Barcode,
                Price = request.Price,
                Description = request.Description,
                TemperatureCondition = request.TemperatureCondition,
                EstimatedWeight = request.EstimatedWeight,
                Observation = request.Observations,
                SubCategory = subCategory,
                Brand = brand,
                Suppliers = suppliers,
                Shelve = shelve,

                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static ProductResponse? ToResponse(Product? product)
        {
            if (product == null) return null;
            return new ProductResponse
            {
                Id = product.Id,
                InternalCode = product.InternalCode,
                Barcode = product.Barcode,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                AvailableStock = product.AvailableStocks,
                Description = product.Description,
                UnitType = product.UnitType,
                TemperatureCondition = product.TemperatureCondition,
                EstimatedWeight = product.EstimatedWeight,
                Observations = product.Observation,
                SubCategory = SubCategoryMapper.ToResponse(product.SubCategory),
                Brand = BrandMapper.ToResponse(product.Brand),
                Suppliers = product.Suppliers?.Select(SupplierMapper.ToResponse).OfType<SupplierResponse>().ToList(),
                Shelve = ShelveMapper.ToResponse(product.Shelve),
                AuditInfo = AuditMapper.ToResponse(product.AuditInfo),
                Discounts = product.Discounts?.Select(DiscountMapper.ToResponse).OfType<DiscountResponse>().ToList()
            };
        }
    }
}
