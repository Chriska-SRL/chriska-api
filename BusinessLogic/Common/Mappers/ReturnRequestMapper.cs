using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsReturnRequest;

namespace BusinessLogic.Common.Mappers
{
    public class ReturnRequestMapper
    {
        public static ReturnRequest ToDomain(ReturnRequestAddRequest request,Delivery delivery, User user)
        {
            ReturnRequest returnRequest = new ReturnRequest(
                observation: request.Observation,
                user: user,
                delivery: delivery
            );

            returnRequest.AuditInfo?.SetCreated(request.getUserId(), request.Location);
            return returnRequest;
        }
        public static ReturnRequestResponse ToResponse(ReturnRequest request)
        {
            return new ReturnRequestResponse
            {
                Id = request.Id,
                Client = ClientMapper.ToResponse(request.Client),
                Status = request.Status,
                Date = request.Date,
                ConfirmedDate = request.ConfirmedDate,
                Observation = request.Observations,
                User = UserMapper.ToResponse(request.User),
                ProductItems = request.ProductItems.Select(ProductItemMapper.ToResponse).ToList(),
                Delivery = DeliveryMapper.ToResponse(request.Delivery),
                AuditInfo = AuditMapper.ToResponse(request.AuditInfo)
            };
        }
       
    }
}
