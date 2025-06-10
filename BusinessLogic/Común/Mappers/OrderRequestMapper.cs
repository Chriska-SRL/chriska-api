using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.Común.Mappers
{
    public static class OrderRequestMapper
    {
        public static OrderRequest ToDomain(AddOrderRequest_Request orderRequest)
        {

            return new OrderRequest(

                id: 0,
                order: null,
                requestDate: orderRequest.RequestDate,
                deliveryDate: orderRequest.DeliveryDate,
                status: orderRequest.Status,
                observation: "",
                user: new User(orderRequest.UserId),                 
                client: new Client(orderRequest.ClientId),
                requestsItems: new List<RequestItem>()
            );
        }
        public static OrderRequest.UpdatableData ToDomain(UpdateOrderRequest_Request updateOrderRequest)
        {
            return new OrderRequest.UpdatableData
            {
                RequestDate = updateOrderRequest.RequestDate,
                DeliveryDate = updateOrderRequest.DeliveryDate,
                Status = updateOrderRequest.Status,
                Observation = updateOrderRequest.Observation,
                User = new User(updateOrderRequest.UserId),
                Client = new Client(updateOrderRequest.ClientId)
            };
        }
        public static OrderRequestResponse ToResponse(OrderRequest orderRequest)
        {
            return new OrderRequestResponse
            {
                RequestDate = orderRequest.RequestDate,
                DeliveryDate = orderRequest.DeliveryDate,
                Status = orderRequest.Status,
                Observation = orderRequest.Observation,
                User = UserMapper.ToResponse(orderRequest.User),
                Client = ClientMapper.ToResponse(orderRequest.Client),
            };
        }
    }
}
