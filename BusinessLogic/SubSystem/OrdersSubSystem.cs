using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class OrdersSubSystem
    {
        private List<Order> Orders = new List<Order>();
        private List<OrderItem> OrderItems = new List<OrderItem>();

        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;
        public OrdersSubSystem(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        public void AddOrder(Order order)
        {
            _orderRepository.Add(order);
        }
        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItemRepository.Add(orderItem);
        }


    }
}
