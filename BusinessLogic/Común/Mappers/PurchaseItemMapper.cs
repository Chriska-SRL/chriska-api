using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsPurchaseItem;

namespace BusinessLogic.Común.Mappers
{
    public static class PurchaseItemMapper
    {
        public static PurchaseItem ToDomain(AddPurchaseItemRequest dto)
        {
            return new PurchaseItem(
                id: 0,
                quantity: dto.Quantity,
                unitPrice: dto.UnitPrice,
                product: new Product(dto.ProductId)
            );
        }

        public static PurchaseItem.UpdatableData ToDomain(UpdatePurchaseItemRequest dto)
        {
            return new PurchaseItem.UpdatableData
            {
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };
        }

        public static PurchaseItemResponse ToResponse(PurchaseItem purchaseItem)
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
