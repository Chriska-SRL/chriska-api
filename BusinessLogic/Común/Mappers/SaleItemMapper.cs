using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSaleItem;

namespace BusinessLogic.Común.Mappers
{
    public static class SaleItemMapper
    {
        public static SaleItem ToDomain(SaleItem sale)
        {
            return new SaleItem(
                id: 0,
                quantity: sale.Quantity,
                unitPrice: sale.UnitPrice,
                product: new Product(
                    id: sale.Product.Id,
                    internalCode: string.Empty,
                    barcode: string.Empty,
                    name: string.Empty,
                    price: 0,
                    image: string.Empty,
                    stock: 0,
                    description: string.Empty,
                    unitType: string.Empty,
                    temperatureCondition: string.Empty,
                    observation: string.Empty,
                    subCategory: null,
                    suppliers: new List<Supplier>()
                )
            );
        }
        public static SaleItem.UpdatableData ToDomain(SaleItem.UpdatableData sale)
        {
            return new SaleItem.UpdatableData
            {
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                Product = new Product(
                    id: sale.Product.Id,
                    internalCode: string.Empty,
                    barcode: string.Empty,
                    name: string.Empty,
                    price: 0,
                    image: string.Empty,
                    stock: 0,
                    description: string.Empty,
                    unitType: string.Empty,
                    temperatureCondition: string.Empty,
                    observation: string.Empty,
                    subCategory: null,
                    suppliers: new List<Supplier>()
                )
            };
        }
        public static SaleItemResponse toResponse(SaleItem domain)
        {
            return new SaleItemResponse
            {
                Id = domain.Id,
                Quantity = domain.Quantity,
                UnitPrice = domain.UnitPrice,
                Product = new ProductResponse
                {
                    Id = domain.Product.Id,
                    InternalCode = domain.Product.InternalCode,
                    Barcode = domain.Product.Barcode,
                    Name = domain.Product.Name,
                    Price = domain.Product.Price,
                    Image = domain.Product.Image,
                    Stock = domain.Product.Stock,
                    Description = domain.Product.Description,
                    UnitType = domain.Product.UnitType,
                    TemperatureCondition = domain.Product.TemperatureCondition,
                    Observation = domain.Product.Observation,
                    SubCategory = new DTOs.DTOsSubCategory.SubCategoryResponse
                    {
                        Id = domain.Product.SubCategory.Id,
                        Name = domain.Product.SubCategory.Name
                    },
                }
            };
        }
    }
}
