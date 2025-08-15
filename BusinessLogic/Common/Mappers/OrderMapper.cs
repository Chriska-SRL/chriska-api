using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderRequest;

namespace BusinessLogic.Common.Mappers
{
    public static class OrderMapper
    {
        public static Order.UpdatableData ToUpdatableData(OrderUpdateRequest request, User user, List<ProductItem> productItems)
        {
            return new Order.UpdatableData
            {
                Observations = request.Observations,
                User = user,
                ProductItems = productItems,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static OrderResponse? ToResponse(Order? order)
        {
            throw new NotImplementedException("ToResponse method is not implemented yet.");
        }
    }
}
