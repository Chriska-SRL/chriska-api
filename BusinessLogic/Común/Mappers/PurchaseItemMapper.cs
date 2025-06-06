using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsPurchaseItem;

namespace BusinessLogic.Común.Mappers
{
    public static class PurchaseItemMapper
    {
        public static PurchaseItem toDomain(AddPurchaseItemRequest addPruchaseItemRequest)
        {
            return new PurchaseItem(
                id: 0,
                quantity: addPruchaseItemRequest.Quantity,
                unitPrice: addPruchaseItemRequest.UnitPrice,
                product: new Product(
                    id: addPruchaseItemRequest.ProductId,
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
        public static PurchaseItem.UpdatableData toUpdatableData(UpdatePurchaseItemRequest purchaseItemResponse)
        {
            return new PurchaseItem.UpdatableData
            {
                Quantity = purchaseItemResponse.Quantity,
                UnitPrice = purchaseItemResponse.UnitPrice
            };
        }
        public static PurchaseItemResponse toResponse(PurchaseItem purchaseItem)
        {
            return new PurchaseItemResponse
            {
                Quantity = purchaseItem.Quantity,
                UnitPrice = purchaseItem.UnitPrice,
                Product = new ProductResponse
                {
                    Id = purchaseItem.Product.Id,
                    InternalCode = purchaseItem.Product.InternalCode,
                    Barcode = purchaseItem.Product.Barcode,
                    Name = purchaseItem.Product.Name,
                    Price = purchaseItem.Product.Price,
                    Image = purchaseItem.Product.Image,
                    Stock = purchaseItem.Product.Stock,
                    Description = purchaseItem.Product.Description,
                    UnitType = purchaseItem.Product.UnitType,
                    TemperatureCondition = purchaseItem.Product.TemperatureCondition,
                    Observation = purchaseItem.Product.Observation
                }
            };
        }
    }
}
