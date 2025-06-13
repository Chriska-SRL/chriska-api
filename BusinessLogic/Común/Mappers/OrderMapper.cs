using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsSale;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.Común.Mappers
{
    public static class OrderMapper
    {
        public static Order ToDomain(AddOrderRequest addOrderRequest)
        {
            return new Order(
                id: 0,
                date: addOrderRequest.Date,
                clientName: addOrderRequest.ClientName,
                crates: addOrderRequest.Crates,
                status: addOrderRequest.Status,
                delivery: new Delivery
                (
                    id: addOrderRequest.DeliveryId,
                    date: DateTime.MinValue,
                    driverName: string.Empty,
                    observation: string.Empty,
                    orders: new List<Order>(),
                    vehicle: null
                ),
                sale: new Sale
                (
                    id: addOrderRequest.SaleId,
                    date: DateTime.MinValue,
                    status: string.Empty,
                    saleItems: new List<SaleItem>()
                ),
                preparedBy: new User
                (
                    id: addOrderRequest.PreparedById,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),
                deliveredBy: new User
                (
                    id: addOrderRequest.DeliveredById,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),
                orderRequest: new OrderRequest
                (
                    id: addOrderRequest.OrderRequestId,
                    order: null,
                    requestDate: DateTime.Now,
                    deliveryDate: DateTime.Now,
                    status: string.Empty,
                    observation: string.Empty,
                    user: null,
                    client: null,
                    requestsItems: new List<RequestItem>()
                ),
                orderItems: new List<OrderItem>()
            );

        }
        public static Order.UpdatableData ToDomain(UpdateOrderRequest updatableOrderRequest)
        {
            return new Order.UpdatableData
            {
                Date = updatableOrderRequest.Date,
                ClientName = updatableOrderRequest.ClientName,
                Crates = updatableOrderRequest.Crates,
                Status = updatableOrderRequest.Status,
                Delivery = new Delivery
                (
                    id: updatableOrderRequest.DeliveryId,
                    date: DateTime.MinValue,
                    driverName: string.Empty,
                    observation: string.Empty,
                    orders: new List<Order>(),
                    vehicle: null
                ),
                Sale = new Sale
                (
                    id: updatableOrderRequest.SaleId,
                    date: DateTime.MinValue,
                    status: string.Empty,
                    saleItems: new List<SaleItem>()
                ),
                PreparedBy = new User
                (
                    id: updatableOrderRequest.PreparedById,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),
                DeliveredBy = new User
                (
                    id: updatableOrderRequest.DeliveredById,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),
                OrderRequest = new OrderRequest
                (
                    id: updatableOrderRequest.OrderRequestId,
                    order: null,
                    requestDate: DateTime.Now,
                    deliveryDate: DateTime.Now,
                    status: string.Empty,
                    observation: string.Empty,
                    user: null,
                    client: null,
                    requestsItems: new List<RequestItem>()
                )
            };
        }
        public static OrderResponse ToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                Date = order.Date,
                ClientName = order.ClientName,
                Crates = order.Crates,
                Status = order.Status,
                Delivery =new DeliveryResponse
                {
                    Id = order.Delivery.Id,
                    Date = order.Delivery.Date,
                    DriverName = order.Delivery.DriverName,
                    Observation = order.Delivery.Observation
                },
                Sale = new SaleResponse
                {
                    Id = order.Sale.Id,
                    Date = order.Sale.Date,
                    Status = order.Sale.Status
                },
                PreparedBy = new UserResponse
                {
                    Id = order.PreparedBy.Id,
                    Name = order.PreparedBy.Name,
                    Username = order.PreparedBy.Username
                },
                DeliveredBy = new UserResponse
                {
                    Id = order.DeliveredBy.Id,
                    Name = order.DeliveredBy.Name,
                    Username = order.DeliveredBy.Username
                },
                OrderRequest = new OrderRequestResponse
                {
                    RequestDate = order.OrderRequest.RequestDate,
                    DeliveryDate = order.OrderRequest.DeliveryDate,
                    Status = order.OrderRequest.Status,
                    Observation = order.OrderRequest.Observation,                   
        }
            };

    }
}
}
