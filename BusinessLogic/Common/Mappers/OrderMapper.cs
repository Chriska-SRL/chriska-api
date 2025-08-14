using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsOrder;

namespace BusinessLogic.Common.Mappers
{
    public class OrderMapper
    {
        public static Order ToDomain()
        {
            // This method should be implemented to convert a request or DTO to an Order domain object.
            throw new NotImplementedException("ToDomain method needs to be implemented.");
        }
        public static OrderResponse ToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                Client = ClientMapper.ToResponse(order.Client),
                Status = order.Status,
                ConfirmedDate = order.ConfirmedDate,
                Date = order.Date,
                Observation = order.Observations,
                User = UserMapper.ToResponse(order.User),
                ProductItems = order.ProductItems.Select(ProductItemMapper.ToResponse).ToList(),
                Crates = order.Crates,
                OrderRequest = order.OrderRequest != null ? OrderRequestMapper.ToResponse(order.OrderRequest) : null,
                Delivery = order.Delivery != null ? DeliveryMapper.ToResponse(order.Delivery) : null,
                AuditInfo = AuditMapper.ToResponse(order.AuditInfo)
            };
        }

    }
}
