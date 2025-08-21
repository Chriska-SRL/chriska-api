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

        public static OrderResponse? ToResponse(Order? order, Boolean notBucle = false)
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
                Observations = order.Observations ?? "",
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
                Delivery = order.Delivery != null ? DeliveryMapper.ToResponse(order.Delivery, true) : null,
                Crates = order.Crates,
                OrderRequest = notBucle ? null : (order.OrderRequest != null ? OrderRequestMapper.ToResponse(order.OrderRequest, true) : null),
                AuditInfo = AuditMapper.ToResponse(order.AuditInfo)
            };

            return response;
        }
    }
}
