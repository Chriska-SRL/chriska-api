using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsOrder;

namespace BusinessLogic.Common.Mappers
{
    public static class OrderMapper
    {
        public static Order ToDomain(OrderAddRequest request, OrderRequest orderRequest, List<ProductItem> productItems, User user)
        {
          throw new NotImplementedException("ToDomain method is not implemented yet.");
        }

        public static Order.UpdatableData ToUpdatableData(OrderUpdateRequest request, User user, List<ProductItem> productItems)
        {
           throw new NotImplementedException("ToUpdatableData method is not implemented yet.");
        }

        public static OrderResponse? ToResponse(Order? order)
        {
            throw new NotImplementedException("ToResponse method is not implemented yet.");
        }
    }
}
