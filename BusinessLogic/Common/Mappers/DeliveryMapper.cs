using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDelivery;

namespace BusinessLogic.Common.Mappers
{
    public class DeliveryMapper
    {
        public static Delivery ToDomain(DeliveryAddRequest request, User user, Order order)
        {
            var delivery = new Delivery(
                observation: request.Observation,
                user: user,
                crates: request.Crates,
                order: order
            );

            delivery.AuditInfo?.SetCreated(request.getUserId(), request.Location);

            return delivery;
        }
        public static DeliveryResponse ToResponse(Delivery delivery)
        {
            return new DeliveryResponse
            {
                Id = delivery.Id,
                Client = ClientMapper.ToResponse(delivery.Client),
                Status = delivery.Status,
                ConfirmedDate = delivery.ConfirmedDate,
                Date = delivery.Date,
                Observation = delivery.Observations,
                User = UserMapper.ToResponse(delivery.User),
                ProductItems = delivery.ProductItems.Select(ProductItemMapper.ToResponse).ToList(),
                Crates = delivery.Crates,
                Order = delivery.Order != null ? OrderMapper.ToResponse(delivery.Order) : null,
                AuditInfo = AuditMapper.ToResponse(delivery.AuditInfo)
            };
        }
    }
}
