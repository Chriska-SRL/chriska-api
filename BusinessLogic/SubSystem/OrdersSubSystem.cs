using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;

namespace BusinessLogic.SubSystem
{
    public class OrdersSubSystem
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrdersSubSystem(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        // Órdenes

        public OrderResponse AddOrder(AddOrderRequest request)
        {
            Order order = OrderMapper.ToDomain(request);
            order.Validate();

            Order added = _orderRepository.Add(order);
            return OrderMapper.ToResponse(added);
        }

        public OrderResponse UpdateOrder(UpdateOrderRequest request)
        {
            Order existing = _orderRepository.GetById(request.Id)
                              ?? throw new InvalidOperationException("Orden no encontrada.");

            Order.UpdatableData updatedData = OrderMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Order updated = _orderRepository.Update(existing);
            return OrderMapper.ToResponse(updated);
        }

        public OrderResponse DeleteOrder(DeleteOrderRequest request)
        {
            Order deleted = _orderRepository.Delete(request.Id)
                              ?? throw new InvalidOperationException("Orden no encontrada.");

            return OrderMapper.ToResponse(deleted);
        }

        public OrderResponse GetOrderById(int id)
        {
            Order order = _orderRepository.GetById(id)
                             ?? throw new InvalidOperationException("Orden no encontrada.");

            return OrderMapper.ToResponse(order);
        }

        public List<OrderResponse> GetAllOrders()
        {
            return _orderRepository.GetAll()
                                   .Select(OrderMapper.ToResponse)
                                   .ToList();
        }

        // Items de orden

        public OrderItemResponse AddOrderItem(AddOrderItemRequest request)
        {
            OrderItem item = OrderItemMapper.ToDomain(request);
            item.Validate();

            OrderItem added = _orderItemRepository.Add(item);
            return OrderItemMapper.ToResponse(added);
        }

        public OrderItemResponse UpdateOrderItem(UpdateOrderItemRequest request)
        {
            OrderItem existing = _orderItemRepository.GetById(request.Id)
                                  ?? throw new InvalidOperationException("Ítem de orden no encontrado.");

            OrderItem.UpdatableData updatedData = OrderItemMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            OrderItem updated = _orderItemRepository.Update(existing);
            return OrderItemMapper.ToResponse(updated);
        }

        public OrderItemResponse DeleteOrderItem(DeleteOrderItemRequest request)
        {
            OrderItem deleted = _orderItemRepository.Delete(request.Id)
                                 ?? throw new InvalidOperationException("Ítem de orden no encontrado.");

            return OrderItemMapper.ToResponse(deleted);
        }

        public OrderItemResponse GetItemOrderById(int id)
        {
            OrderItem orderItem = _orderItemRepository.GetById(id)
                                   ?? throw new InvalidOperationException("Ítem de orden no encontrado.");

            return OrderItemMapper.ToResponse(orderItem);
        }

        public List<OrderItemResponse> GetAllOrderItems()
        {
            return _orderItemRepository.GetAll()
                                       .Select(OrderItemMapper.ToResponse)
                                       .ToList();
        }
    }
}
