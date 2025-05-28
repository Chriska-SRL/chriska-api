using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSale;

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

        private readonly UserSubSystem _userSubSystem;
        private readonly DeliveriesSubSystem _deliveriesSubSystem;
        public readonly ReturnsSubSystem subSystemReturnRequest;

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
       public OrderResponse GetOrderById(int id)
       {
           var order = _orderRepository.GetById(id);
           if (order == null) throw new Exception("No se encontro la orden");
           var Sale = _saleRepository.GetById(order.Sale.Id);
            var orderResponse= new OrderResponse{


               Id = order.Id,
               Date = order.Date,
               ClientName = order.ClientName,
               Crates = order.Crates,
               Status = order.Status,
               Delivery = _deliveriesSubSystem.GetById(order.Delivery.Id),
               Sale = new SaleResponse{
                   Status = Sale.Status,
                   Date = Sale.Date,
               },         
               PreparedBy = _userSubSystem.GetUserById(order.PreparedBy.Id),
               DeliveredBy = _userSubSystem.GetUserById(order.DeliveredBy.Id),
               OrderRequest = subSystemReturnRequest.GetOrderRequestById(order.OrderRequest.Id)


               };
            return orderResponse;
       }
       
        public List<OrderResponse> GetAllOrders()
        {
            var list = _orderRepository.GetAll();
            if (list == null) throw new Exception("No se encontraron ordenes");
            var listResponse = new List<OrderResponse>();
            foreach (var order in list)
            {
                var response = new OrderResponse
                {
                    Id = order.Id,
                    Date = order.Date,
                    ClientName = order.ClientName,
                    Crates = order.Crates,
                    Status = order.Status,
                    Delivery = _deliveriesSubSystem.GetById(order.Delivery.Id),
                    Sale = new SaleResponse
                    {
                        Status = order.Status,
                        Date = order.Date,
                    },
                    PreparedBy = _userSubSystem.GetUserById(order.PreparedBy.Id),
                    DeliveredBy = _userSubSystem.GetUserById(order.DeliveredBy.Id),
                    OrderRequest = subSystemReturnRequest.GetOrderRequestById(order.OrderRequest.Id)


                }; 
                listResponse.Add(response);
            }
            return listResponse;
        }
        
        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItemRepository.Add(orderItem);
        }
       
      
       
        public OrderItemResponse GetItemOrderById(int id)
        {
            
            var orderItem = _orderItemRepository.GetById(id);
            if (orderItem == null) throw new Exception("No se encontro el item de orden");
            var orderItemResponse = new OrderItemResponse
            {

                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                Product = new ProductResponse
                {
                    Id = orderItem.Product.Id,
                    Name = orderItem.Product.Name,
                    Description = orderItem.Product.Description,
                    Price = orderItem.Product.Price,
                    Stock = orderItem.Product.Stock
                }
            };
     
            return orderItemResponse;

        }

    }
}
