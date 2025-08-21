using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class OrderSubSystem
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryRepository _deliveriesRepository;
        private readonly DeliveriesSubSystem _deliveriesSubSystem;

        public OrderSubSystem(IOrderRepository orderRepository, IClientRepository clientRepository, IProductRepository productRepository, IUserRepository userRepository, DeliveriesSubSystem deliveriesSubSystem, IOrderRequestRepository orderRequestRepository, IDeliveryRepository deliveriesRepository)
        {
            _orderRepository = orderRepository;
            _orderRequestRepository = orderRequestRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _deliveriesSubSystem = deliveriesSubSystem;
            _deliveriesRepository = deliveriesRepository;
        }

        public async Task<Order?> AddOrderAsync(OrderRequest orderRequest)
        {
            orderRequest.Validate();
            Order order = new Order(orderRequest);
            order.AuditInfo.SetCreated(orderRequest.AuditInfo.UpdatedBy, orderRequest.AuditInfo.UpdatedLocation);
            await _orderRepository.AddAsync(order);
            return order;
        }

        public async Task<OrderResponse> UpdateOrderAsync(OrderUpdateRequest request)
        {
            var existing = await _orderRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {request.Id}.");
            existing.OrderRequest = await _orderRequestRepository.GetByIdAsync(existing.Id)
               ?? throw new ArgumentException("No se encontró la solicitud de pedido asociada a la orden.");

            if (existing.Status != Status.Pending)
                throw new ArgumentException("La orden no se puede modificar porque no está en estado pendiente.");

            var userId = request.getUserId() ?? 0;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la modificación no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                ProductItem? productitem = existing.OrderRequest.ProductItems.FirstOrDefault(p => p.Product.Id == item.ProductId);
                if (productitem != null)
                {
                    // Si el producto ya existe, actualizamos la cantidad y peso
                    productitem.Quantity = item.Quantity;
                    productitem.Weight = item.Weight ?? 0;
                    productItems.Add(productitem);
                }
                else
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId)
                                           ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");
                    Discount? discount = product.GetBestDiscount(existing.Client);
                    decimal discountPercentage = discount?.Percentage ?? 0;
                    productItems.Add(new ProductItem(item.Quantity, item.Weight ?? 0, product.Price, discountPercentage, product));
                }
            }

            Order.UpdatableData updatedData = OrderMapper.ToUpdatableData(request, user, productItems);
            existing.Update(updatedData);
            existing.Crates = request.Crates;

            var updated = await _orderRepository.UpdateAsync(existing);
            return OrderMapper.ToResponse(updated);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {id}.");
            order.OrderRequest = await _orderRequestRepository.GetByIdAsync(order.Id)
              ?? throw new ArgumentException("No se encontró la solicitud de pedido asociada a la orden.");
            order.Delivery = await _deliveriesRepository.GetByIdAsync(order.Id);
            return OrderMapper.ToResponse(order);
        }

        public async Task<List<OrderResponse?>> GetAllOrdersAsync(QueryOptions options)
        {
            List<Order> orders = await _orderRepository.GetAllAsync(options);
            return orders.Select(o => OrderMapper.ToResponse(o)).ToList();
        }

        internal async Task<OrderResponse?> ChangeStatusOrderAsync(int id, DocumentClientChangeStatusRequest request)
        {
            Order? order = await _orderRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {id}.");
            order.OrderRequest = await _orderRequestRepository.GetByIdAsync(order.Id)
                ?? throw new ArgumentException("No se encontró la solicitud de pedido asociada a la orden.");
            if (request.Status == Status.Pending)
                throw new ArgumentException("no se puede cambiar a Pending");
            if (order.Status != Status.Pending)
                throw new ArgumentException("La orden no se puede cambiar de estado porque no está en estado pendiente.");

            int userId = request.getUserId() ?? 0;
            User? user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza el cambio de estado no existe.");
            order.AuditInfo.SetUpdated(userId, request.Location);
            order.User = user;

            Delivery delivery = null;
            if (request.Status == Status.Confirmed)
            {
                order.Confirm();
                delivery = await _deliveriesSubSystem.AddDeliveryAsync(order);
            }
            else if (request.Status == Status.Cancelled)
            {
                order.Cancel();
            }
            else
            {
                throw new ArgumentException("El estado de la orden no es válido para cambiar.");
            }

            order.Delivery = delivery;
            order = await _orderRepository.ChangeStatusOrder(order);
            return OrderMapper.ToResponse(order);
        }
    }
}
