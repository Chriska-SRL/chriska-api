using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class OrderRequestSubSystem
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly OrderSubSystem _orderSubSystem;
        public OrderRequestSubSystem(IOrderRequestRepository orderRequestRepository, IClientRepository clientRepository, IProductRepository productRepository, IUserRepository userRepository, OrderSubSystem orderSubSystem, IDiscountRepository discountRepository)
        {
            _orderRequestRepository = orderRequestRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _discountRepository = discountRepository;
            _orderSubSystem = orderSubSystem;
        }
        public async Task<OrderRequestResponse?> AddOrderRequestAsync(OrderRequestAddRequest request)
        {
            Client client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("El cliente seleccionado no existe.");

            int userId = request.getUserId() ?? 0;
            User user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la solicitud no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                Product product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");

                Discount? discount = await _discountRepository.GetBestByProductAndClientAsync(product, client);
                decimal discountPercentage = 0;
                if (discount != null && item.Quantity >= discount.ProductQuantity)
                {
                    discountPercentage = discount.Percentage;
                }
                productItems.Add(new ProductItem(item.Quantity, 0, product.Price, discountPercentage, product));
            }


            OrderRequest newOrderRequest = OrderRequestMapper.ToDomain(request, client, productItems, user);

            return OrderRequestMapper.ToResponse(await _orderRequestRepository.AddAsync(newOrderRequest));
        }

        public async Task<OrderRequestResponse> UpdateOrderRequestAsync(OrderRequestUpdateRequest request)
        {
            var existing = await _orderRequestRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una solicitud de pedido con el ID {request.Id}.");

            if (existing.Status != Status.Pending)
                throw new ArgumentException("La solicitud de pedido no se puede modificar porque no está en estado pendiente.");

            var client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("El cliente seleccionado no existe.");

            var userId = request.getUserId() ?? 0;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la modificación no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                Product product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");

                Discount? discount = await _discountRepository.GetBestByProductAndClientAsync(product, client);
                decimal discountPercentage = 0;
                if(discount != null && item.Quantity >= discount.ProductQuantity)
                {
                    discountPercentage = discount.Percentage;
                }
                productItems.Add(new ProductItem(item.Quantity, 0, product.Price, discountPercentage, product));
            }

            OrderRequest.UpdatableData updatedData = OrderRequestMapper.ToUpdatableData(request, user, productItems);
            existing.Update(updatedData);
            existing.Client = client;

            var updated = await _orderRequestRepository.UpdateAsync(existing);
            return OrderRequestMapper.ToResponse(updated);
        }

        public async Task<OrderRequestResponse> DeleteOrderRequestAsync(DeleteRequest request)
        {
            var existing = await _orderRequestRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una solicitud de pedido con el ID {request.Id}.");

            existing.MarkAsDeleted(request.getUserId(), request.Location);
            var deleted = await _orderRequestRepository.DeleteAsync(existing);
            return OrderRequestMapper.ToResponse(deleted);
        }


        public async Task<OrderRequestResponse?> GetOrderRequestByIdAsync(int id)
        {
            OrderRequest? orderRequest = await _orderRequestRepository.GetByIdAsync(id);
            if (orderRequest == null)
            {
                throw new ArgumentException($"No se encontró una solicitud de pedido con el ID {id}.");
            }
            return OrderRequestMapper.ToResponse(orderRequest);
        }

        public async Task<List<OrderRequestResponse?>> GetAllOrderRequestsAsync(QueryOptions options)
        {
            List<OrderRequest> orderRequests = await _orderRequestRepository.GetAllAsync(options);
            return orderRequests.Select(or => OrderRequestMapper.ToResponse(or)).ToList();
        }

        internal async Task<OrderRequestResponse?> ChangeStatusOrderRequestAsync(int id, DocumentClientChangeStatusRequest request)
        {
            OrderRequest orderRequest = await _orderRequestRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una solicitud de pedido con el ID {id}.");

            if (orderRequest.Status != Status.Pending)
                throw new ArgumentException("La solicitud de pedido no se puede modificar porque no está en estado pendiente.");

            int userId = request.getUserId() ?? 0;
            User? user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza el cambio de estado no existe.");
            orderRequest.AuditInfo.SetUpdated(userId, request.Location);
            orderRequest.User = user;
            Order order = null;
            if (request.Status == Status.Confirmed)
            {
                orderRequest.Confirm();

                foreach (var item in orderRequest.ProductItems)
                {
                    await _productRepository.UpdateStockAsync(item.Product.Id, 0,-item.Quantity);
                }

                order = await _orderSubSystem.AddOrderAsync(orderRequest);
            }
            else if (request.Status == Status.Cancelled)
            {
                orderRequest.Cancel();
                foreach (var item in orderRequest.ProductItems)
                {
                    await _productRepository.UpdateStockAsync(item.Product.Id, 0, item.Quantity);
                }
            }
            else
            {
                throw new ArgumentException("El estado de la solicitud de pedido no es válido para cambiar.");
            }
            
            orderRequest.Order = order;
            orderRequest = await _orderRequestRepository.ChangeStatusOrderRequest(orderRequest);
            return OrderRequestMapper.ToResponse(orderRequest);
        }
    }
}
