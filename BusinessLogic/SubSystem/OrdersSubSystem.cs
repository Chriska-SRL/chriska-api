using BusinessLogic.Dominio;
using BusinessLogic.DTOsOrder;
using BusinessLogic.DTOsOrderItem;
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

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReturnRequestRepository _returnRequestRepository;

        public OrdersSubSystem(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository,IDeliveryRepository deliveryRepository,ISaleRepository saleRepository, IUserRepository userRepository,IReturnRequestRepository returnRequestRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _deliveryRepository = deliveryRepository;
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _returnRequestRepository = returnRequestRepository;
        }
        public void AddOrder(AddOrderRequest orderRequest)
        {
            var order = new Order( orderRequest.Date, orderRequest.ClientName, orderRequest.Crates, orderRequest.Status, _deliveryRepository.GetById(orderRequest.DeliveryId), _saleRepository.GetById(orderRequest.SaleId), _userRepository.GetById(orderRequest.PreparedById), _userRepository.GetById(orderRequest.DeliveredById), _returnRequestRepository.GetById(orderRequest.OrderRequestId));
            order.Validate();
            _orderRepository.Add(order);

        }
        public void UpdateOrder(UpdateOrderRequest orderRequest)
        {
            var order = _orderRepository.GetById(orderRequest.Id);
            if (order == null) throw new Exception("No se encontro la orden");
            order.Update(orderRequest.ClientName, orderRequest.Crates, orderRequest.Status, _deliveryRepository.GetById(orderRequest.DeliveryId));
            _orderRepository.Update(order);
           
        }
        public void DeleteOrder(DeleteOrderRequest orderRequest)
        {
            var order = _orderRepository.GetById(orderRequest.Id);
            if (order == null) throw new Exception("No se encontro la orden");
            _orderRepository.Delete(orderRequest.Id);
        }
        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItemRepository.Add(orderItem);
        }


    }
}
