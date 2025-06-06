using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsRequestItem;
using BusinessLogic.DTOs.DTOsSubCategory;


namespace BusinessLogic.Común.Mappers
{
    public static class RequestItemMapper
    {
        public static RequestItem ToDomain(AddRequestItem_Request addRequestItemRequest)
        {
            return new RequestItem(
                id: 0,
                quantity: addRequestItemRequest.Quantity,
                unitPrice: addRequestItemRequest.UnitPrice,
                product: new Product(
                    id: addRequestItemRequest.ProductId,
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
        public static RequestItem.UpdatableData ToDomain(UpdateRequestItem_Request update)
        {
            return new RequestItem.UpdatableData
            {
                Quantity = update.Quantity,
                UnitPrice = update.UnitPrice,
            };
        }
        public static RequestItemResponse toResponse(RequestItem requestItem)
        {
            return new RequestItemResponse
            {
                Quantity = requestItem.Quantity,
                UnitPrice = requestItem.UnitPrice,
                Product = new ProductResponse
                {
                    Id = requestItem.Product.Id,
                    InternalCode = requestItem.Product.InternalCode,
                    Barcode = requestItem.Product.Barcode,
                    Name = requestItem.Product.Name,
                    Price = requestItem.Product.Price,
                    Image = requestItem.Product.Image,
                    Stock = requestItem.Product.Stock,
                    Description = requestItem.Product.Description,
                    UnitType = requestItem.Product.UnitType,
                    TemperatureCondition = requestItem.Product.TemperatureCondition,
                    Observation = requestItem.Product.Observation,
                    SubCategory = new SubCategoryResponse
                    {
                        Id = requestItem.Product.SubCategory.Id,
                        Name = requestItem.Product.SubCategory.Name
                    }
                }
            };
        }
    }     
}
