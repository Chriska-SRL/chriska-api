using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsReturnRequest;

namespace BusinessLogic.Common.Mappers
{
    public class DeliveryMapper
    {
        public static Delivery ToDomain(DeliveryAddRequest request, User user, Order order)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public static Delivery.UpdatableData ToUpdatableData(DeliveryUpdateRequest request)
        {
            return new Delivery.UpdatableData
            {
                Observations = request.Observations,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }
        public static DeliveryResponse ToResponse(Delivery delivery, Boolean notBucle = false)
        {
            return new DeliveryResponse
            {
                Id = delivery.Id,
                Client = ClientMapper.ToResponse(delivery.Client),
                Status = delivery.Status,
                ConfirmedDate = delivery.ConfirmedDate,
                Date = delivery.Date,
                Observations = delivery.Observations,
                User = UserMapper.ToResponse(delivery.User),
                ProductItems = delivery.ProductItems.Select(ProductItemMapper.ToResponse).ToList(),
                Crates = delivery.Crates,
                Order = notBucle ? null : (delivery.Order != null ? OrderMapper.ToResponse(delivery.Order) : null),
                AuditInfo = AuditMapper.ToResponse(delivery.AuditInfo)
            };
        }
    }
}
