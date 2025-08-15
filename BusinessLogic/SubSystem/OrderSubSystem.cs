using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
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

        public async Task<OrderResponse?> AddOrderAsync(OrderAddRequest request)
        {
            Client client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("El cliente seleccionado no existe.");

            int userId = request.getUserId() ?? 0;
            User user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la operación no existe.");

            List<ProductItem> productItems = new();
            foreach (var item in request.ProductItems)
            {
                Product product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");

                decimal discount = product.getDiscount(client);
                productItems.Add(new ProductItem(item.Quantity, item.Weight, product.Price, discount, product));
            }

            Order newOrder = OrderMapper.ToDomain(request, client, productItems, user);
            return OrderMapper.ToResponse(await _orderRepository.AddAsync(newOrder));
        }

        public async Task<OrderResponse> UpdateOrderAsync(OrderUpdateRequest request)
        {
            var existing = await _orderRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {request.Id}.");

            var client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("El cliente seleccionado no existe.");

            var userId = request.getUserId() ?? 0;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la modificación no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");

                decimal discount = product.getDiscount(client);
                productItems.Add(new ProductItem(item.Quantity, item.Weight, product.Price, discount, product));
            }

            Order.UpdatableData updatedData = OrderMapper.ToUpdatableData(request, user, productItems);
            existing.Update(updatedData);
            existing.Client = client;

            var updated = await _orderRepository.UpdateAsync(existing);
            return OrderMapper.ToResponse(updated);
        }

        public async Task<OrderResponse> DeleteOrderAsync(DeleteRequest request)
        {
            var existing = await _orderRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {request.Id}.");

            existing.MarkAsDeleted(request.getUserId(), request.Location);
            var deleted = await _orderRepository.DeleteAsync(existing);
            return OrderMapper.ToResponse(deleted);
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

        internal async Task<OrderResponse?> ChangeStatusOrderAsync(int id, OrderChangeStatusRequest request)
        {
            Order? order = await _orderRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {id}.");

            order.AuditInfo.SetUpdated(request.getUserId(), request.Location);

            if (request.Status == Status.Confirmed)
            {
                order.Confirm();
            }
            else if (request.Status == Status.Canceled)
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
