using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class OrderRequestSubSystem
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _UserRepository;
        public OrderRequestSubSystem(IOrderRequestRepository orderRequestRepository, IClientRepository clientRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRequestRepository = orderRequestRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _UserRepository = userRepository;
        }
        public async Task<OrderRequestResponse?> AddOrderRequestAsync(OrderRequestAddRequest request)
        {
            Client client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("El cliente seleccionado no existe.");

            int userId = request.getUserId() ?? 0;
            User user = await _UserRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la solicitud no existe.");

            List<ProductItem> productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                Product product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");

                decimal discount = product.getDiscount(client);

                productItems.Add(new ProductItem(item.Quantity, item.Weight, product.Price, discount, product));
            }


            OrderRequest newOrderRequest = OrderRequestMapper.ToDomain(request, client, productItems, user);

            return OrderRequestMapper.ToResponse(await _orderRequestRepository.AddAsync(newOrderRequest));
        }

        public async Task<OrderRequestResponse> UpdateOrderRequestAsync(OrderRequestUpdateRequest request)
        {
            throw new NotImplementedException("UpdateOrderRequestAsync is not implemented yet.");
        }

        public async Task<OrderRequestResponse> DeleteOrderRequestAsync(DeleteRequest request)
        {
           throw new NotImplementedException("DeleteOrderRequestAsync is not implemented yet.");
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
            if (orderRequests == null || orderRequests.Count == 0)
            {
                throw new ArgumentException("No se encontraron solicitudes de pedido.");
            }
            return orderRequests.Select(OrderRequestMapper.ToResponse).ToList();
        }
    }
}
