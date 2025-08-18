using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDelivery;
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
        private readonly VehicleSubSystem _vehicleSubSystem;
        

        public OrderSubSystem(IOrderRepository orderRepository, IClientRepository clientRepository, IProductRepository productRepository, IUserRepository userRepository, VehicleSubSystem vehicleSubSystem)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _vehicleSubSystem = vehicleSubSystem;
        }   

        internal async Task<OrderResponse?> ChangeStatusOrderAsync(int id, DocumentClientChangeStatusRequest request)
        {
            Order? order = await _orderRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una orden con el ID {id}.");

            if (order.Status != Status.Pending)
                throw new ArgumentException("La orden no se puede cambiar de estado porque no está en estado pendiente.");

            order.AuditInfo.SetUpdated(request.getUserId(), request.Location);

            if (request.Status == Status.Confirmed)
            {
                order.Confirm();

                // Crear Delivery al estilo OrderRequestSubSystem
                var deliveryRequest = new DeliveryAddRequest
                {
                    OrderId = order.Id,
                    Observation = order.Observations,
                    Crates = order.Crates,
                    UserId = order.User.Id 
                };

                deliveryRequest.setUserId(order.User?.Id ?? throw new ArgumentException("El usuario de la orden no está definido"));
                // Guardar Delivery en DB
                await _vehicleSubSystem.AddDeliveryAsync(deliveryRequest);

                // Asociar Delivery al Order
               // order.Delivery = newDelivery;
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