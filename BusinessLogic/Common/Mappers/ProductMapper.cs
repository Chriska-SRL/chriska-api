using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsBrand;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(ProductAddRequest request, SubCategory subCategory, Brand brand, List<Supplier> suppliers)
        {
            Product product = new Product(
                barcode: request.Barcode,
                name: request.Name,
                price: request.Price,
                description: request.Description,
                unitType: request.UnitType,
                temperatureCondition: request.TemperatureCondition,
                estimatedWeight: request.EstimatedWeight,
                observations: request.Observations,
                subCategory: subCategory,
                brand: brand,
                suppliers: suppliers
            );

            product.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return product;
        }

        public static Product.UpdatableData ToUpdatableData(ProductUpdateRequest request, SubCategory subCategory, Brand brand, List<Supplier> suppliers)
        {
            return new Product.UpdatableData
            {
                Name = request.Name,
                Barcode = request.Barcode,
                Price = request.Price,
                Description = request.Description,
                UnitType = request.UnitType,
                TemperatureCondition = request.TemperatureCondition,
                EstimatedWeight = request.EstimatedWeight,
                Observation = request.Observations,
                SubCategory = subCategory,
                Brand = new Brand(request.BrandId),
                Suppliers = suppliers,

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
                Suppliers = product.Suppliers.Select(SupplierMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(product.AuditInfo)
            };
        }
    }
}
