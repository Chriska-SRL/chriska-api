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
        // Guía temporal: entidades que maneja este subsistema

        private List<Order> Orders = new List<Order>();
        private List<OrderItem> OrderItems = new List<OrderItem>();

        private IOrderRepository _orderRepository;
        public OrdersSubSystem(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


    }
}
