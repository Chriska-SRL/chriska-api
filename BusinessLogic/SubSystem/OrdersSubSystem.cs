using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Común.Mappers;

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

        public void AddOrder(AddOrderRequest orderRequest)
        {
            Order newOrder = OrderMapper.ToDomain(orderRequest);
            newOrder.Validate();
            _orderRepository.Add(newOrder);
        }

        public void UpdateOrder(UpdateOrderRequest orderRequest)
        {
            Order exisitingOrder = _orderRepository.GetById(orderRequest.Id);
            if (exisitingOrder == null) throw new Exception("No se encontro la orden");
            exisitingOrder.Update(OrderMapper.ToDomain(orderRequest));
            _orderRepository.Update(exisitingOrder);
        }

        public void DeleteOrder(DeleteOrderRequest orderRequest)
        {
            var order = _orderRepository.GetById(orderRequest.Id);
            if (order == null) throw new Exception("No se encontro la orden");
            _orderRepository.Delete(orderRequest.Id);
        }

       public OrderResponse GetOrderById(int id)
       {
           Order order = _orderRepository.GetById(id);
           if (order == null) throw new Exception("No se encontro la orden");
           OrderResponse orderResponse = OrderMapper.ToResponse(order);
            return orderResponse;
        }
       
        public List<OrderResponse> GetAllOrders()
        {
            List<Order> orders = _orderRepository.GetAll();
            if (!orders.Any()) throw new Exception("No se encontraron ordenes");
            return orders.Select(OrderMapper.ToResponse).ToList();
        }  
        public void AddOrderItem(AddOrderItemRequest orderItem)
        {
            OrderItem newOrderItem = OrderItemMapper.ToDomain(orderItem);
            newOrderItem.Validate();
            _orderItemRepository.Add(newOrderItem);
        }
        public void UpdateOrderItem(UpdateOrderItemRequest orderItem)
        {
            OrderItem existingOrderItem = _orderItemRepository.GetById(orderItem.Id);
            if (existingOrderItem == null) throw new Exception("No se encontro el item de orden");
            existingOrderItem.Update(OrderItemMapper.ToDomain(orderItem));
            _orderItemRepository.Update(existingOrderItem);
        }
        public void DeleteOrderItem(DeleteOrderItemRequest orderItem)
        {
            var orderItemToDelete = _orderItemRepository.GetById(orderItem.Id);
            if (orderItemToDelete == null) throw new Exception("No se encontro el item de orden");
            _orderItemRepository.Delete(orderItem.Id);
        }
        public OrderItemResponse GetItemOrderById(int id)
        {
            OrderItem orderItem = _orderItemRepository.GetById(id);
            if (orderItem == null) throw new Exception("No se encontro el item de orden");
            OrderItemResponse orderItemResponse = OrderItemMapper.ToResponse(orderItem);
            return orderItemResponse;
        }
        public List<OrderItemResponse> GetAllOrderItems()
        {
            List<OrderItem> orderItems = _orderItemRepository.GetAll();
            if (!orderItems.Any()) throw new Exception("No se encontraron items de orden");
            return orderItems.Select(OrderItemMapper.ToResponse).ToList();
        }
    }
}
