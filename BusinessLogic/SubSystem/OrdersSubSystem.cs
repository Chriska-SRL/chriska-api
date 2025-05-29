using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSale;

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
        private readonly IProductRepository _productRepository;

        private readonly UserSubSystem userSubSystem;
        private readonly DeliveriesSubSystem deliveriesSubSystem;
        private readonly ReturnsSubSystem subSystemReturnRequest;
        public OrdersSubSystem(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository,IDeliveryRepository deliveryRepository,ISaleRepository saleRepository, IUserRepository userRepository,IReturnRequestRepository returnRequestRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _deliveryRepository = deliveryRepository;
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _returnRequestRepository = returnRequestRepository;
            _productRepository = productRepository;
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
               Delivery = deliveriesSubSystem.GetDeliveryById(order.Delivery.Id),
               Sale = new SaleResponse{
                   Status = Sale.Status,
                   Date = Sale.Date,
               },         
               PreparedBy = userSubSystem.GetUserById(order.PreparedBy.Id),
               DeliveredBy = userSubSystem.GetUserById(order.DeliveredBy.Id),
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
                    Delivery = deliveriesSubSystem.GetDeliveryById(order.Delivery.Id),
                    Sale = new SaleResponse
                    {
                        Status = order.Status,
                        Date = order.Date,
                    },
                    PreparedBy = userSubSystem.GetUserById(order.PreparedBy.Id),
                    DeliveredBy = userSubSystem.GetUserById(order.DeliveredBy.Id),
                    OrderRequest = subSystemReturnRequest.GetOrderRequestById(order.OrderRequest.Id)
                }; 
                listResponse.Add(response);
            }
            return listResponse;
        }
        public void AddOrderItem(AddOrderItemRequest orderItemRequest)
        {
            var orderItem = new OrderItem(orderItemRequest.Quantity, orderItemRequest.UnitPrice, _productRepository.GetById(orderItemRequest.ProductId));
            orderItem.Validate();
            _orderItemRepository.Add(orderItem);

        }
        public void UpdateOrderItem(UpdateOrderItemRequest orderItemRequest)
        {
            var orderItem = _orderItemRepository.GetById(orderItemRequest.Id);
            if (orderItem == null) throw new Exception("No se encontro el item de orden");
            orderItem.Update(orderItemRequest.Quantity, orderItemRequest.UnitPrice, _productRepository.GetById(orderItemRequest.ProductId));
            _orderItemRepository.Update(orderItem);
        }
        public void DeleteOrderItem(DeleteOrderItemRequest orderItemRequest)
        {
            var orderItem = _orderItemRepository.GetById(orderItemRequest.Id);
            if (orderItem == null) throw new Exception("No se encontro el item de orden");
            _orderItemRepository.Delete(orderItemRequest.Id);
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
