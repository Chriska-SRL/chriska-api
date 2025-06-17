using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsOrder;

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
                delivery: new Delivery(addOrderRequest.DeliveryId),
                sale: new Sale(addOrderRequest.SaleId),
                preparedBy: new User(addOrderRequest.PreparedById),
                deliveredBy: new User(addOrderRequest.DeliveredById),
                orderRequest: new OrderRequest(addOrderRequest.OrderRequestId),
                orderItems: new List<OrderItem>()
            );

        }
        public static Order.UpdatableData ToUpdatableData(UpdateOrderRequest updatableOrderRequest)
        {
            return new Order.UpdatableData
            {
                Date = updatableOrderRequest.Date,
                ClientName = updatableOrderRequest.ClientName,
                Crates = updatableOrderRequest.Crates,
                Status = updatableOrderRequest.Status,
                Delivery = new Delivery(updatableOrderRequest.DeliveryId),
                Sale = new Sale(updatableOrderRequest.SaleId),
                PreparedBy = new User(updatableOrderRequest.PreparedById),
                DeliveredBy = new User(id: updatableOrderRequest.DeliveredById),
                OrderRequest = new OrderRequest(updatableOrderRequest.OrderRequestId)
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
                Delivery = DeliveryMapper.ToResponse(order.Delivery),
                Sale = SaleMapper.ToResponse(order.Sale),
                PreparedBy = UserMapper.ToResponse(order.PreparedBy),
                DeliveredBy = UserMapper.ToResponse(order.DeliveredBy),
                OrderRequest = OrderRequestMapper.ToResponse(order.OrderRequest),
            };
        }
    }
}
