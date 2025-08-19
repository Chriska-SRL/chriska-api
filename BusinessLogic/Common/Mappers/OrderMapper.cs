using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsProductItem;

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
            if (order == null)
            {
                return null;
            }

            OrderResponse response = new OrderResponse
            {
                Id = order.Id,
                Date = order.Date,
                ConfirmedDate = order.ConfirmedDate,
                Observation = order.Observations ?? "",
                Status = order.Status,
                Client = ClientMapper.ToResponse(order.Client),
                User = UserMapper.ToResponse(order.User),
                ProductItems = order.ProductItems.Select(pi => new ProductItemResponse
                {
                    Product = ProductMapper.ToResponse(pi.Product),
                    Quantity = pi.Quantity,
                    Weight = pi.Weight,
                    UnitPrice = pi.UnitPrice,
                    Discount = pi.Discount
                }).ToList(),
                //Delivery = DeliveryMapper.ToResponse(order.Delivery),
                Crates = order.Crates,
                //OrderRequest = OrderRequestMapper.ToResponse(order.OrderRequest),
                AuditInfo = AuditMapper.ToResponse(order.AuditInfo)
            };

            return response;
        }
    }
}
