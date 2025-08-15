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
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderSubSystem(IOrderRepository orderRepository, IClientRepository clientRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<Order?> AddOrderAsync(OrderRequest orderRequest)
        {
            orderRequest.Validate();
            Order order = new Order(orderRequest);
            order.AuditInfo.SetCreated(orderRequest.AuditInfo.UpdatedBy, null);
            return order;
        }

        public async Task<OrderResponse> UpdateOrderAsync(OrderUpdateRequest request)
        {
            var existing = await _orderRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {request.Id}.");

            if(existing.Status != Status.Pending)
                throw new ArgumentException("La orden no se puede modificar porque no está en estado pendiente.");

            var client = await _clientRepository.GetByIdAsync(existing.Client.Id)
                ?? throw new ArgumentException("El cliente seleccionado no existe.");

            var userId = request.getUserId() ?? 0;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la modificación no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");

                //TODO: ajustar esta logica
                Discount? discount = product.GetBestDiscount(client);
                decimal discountPercentage = discount?.Percentage ?? 0;
                productItems.Add(new ProductItem(item.Quantity, item.Weight, product.Price, discountPercentage, product));
            }

            Order.UpdatableData updatedData = OrderMapper.ToUpdatableData(request, user, productItems);
            existing.Update(updatedData);
            existing.Crates = request.Crates;
            existing.Client = client;

            var updated = await _orderRepository.UpdateAsync(existing);
            return OrderMapper.ToResponse(updated);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {id}.");
            return OrderMapper.ToResponse(order);
        }

        public async Task<List<OrderResponse?>> GetAllOrdersAsync(QueryOptions options)
        {
            List<Order> orders = await _orderRepository.GetAllAsync(options);
            if (orders == null || orders.Count == 0)
                throw new ArgumentException("No se encontraron órdenes.");
            return orders.Select(OrderMapper.ToResponse).ToList();
        }

        internal async Task<OrderResponse?> ChangeStatusOrderAsync(int id, DocumentClientChangeStatusRequest request)
        {
            Order? order = await _orderRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {id}.");

            if(order.Status != Status.Pending)
                throw new ArgumentException("La orden no se puede cambiar de estado porque no está en estado pendiente.");

            order.AuditInfo.SetUpdated(request.getUserId(), request.Location);

            if (request.Status == Status.Confirmed)
            {
                order.Confirm();
                //TODO: Crear Delivery
            }
            else if (request.Status == Status.Cancelled)
            {
                order.Cancel();
            }
            else
            {
                throw new ArgumentException("El estado de la orden no es válido para cambiar.");
            }

            order = await _orderRepository.ChangeStatusOrder(order);
            return OrderMapper.ToResponse(order);
        }
    }
}
