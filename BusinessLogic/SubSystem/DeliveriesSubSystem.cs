using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs;
using BusinessLogic.Repository;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsReceipt;

namespace BusinessLogic.SubSystem
{
    public class DeliveriesSubSystem
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IUserRepository _userRepository;
        private readonly  IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ClientReceiptSubSystem _receiptSubSystem;

        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository, IUserRepository userRepository, IOrderRepository orderRepository,IProductRepository productRepository, ClientReceiptSubSystem receiptSubSystem, IClientRepository clientRepository)
        {
            _deliveryRepository = deliveryRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _receiptSubSystem = receiptSubSystem;
            _clientRepository = clientRepository;
        }

        public async Task<Delivery> AddDeliveryAsync(Order order)
        {
            order.Validate();
            Delivery delivery = new Delivery(order);
            delivery.AuditInfo.SetCreated(order.AuditInfo.UpdatedBy, order.AuditInfo.UpdatedLocation);
            await _deliveryRepository.AddAsync(delivery);
            return delivery;
     
        }

        public async Task<DeliveryResponse> UpdateDeliveryAsync(DeliveryUpdateRequest request)
        {
            var existing = await _deliveryRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró la entrega con el ID {request.Id}.");

            if (existing.Status != Status.Pending)
                throw new ArgumentException("La entrega no se puede modificar porque no está en estado pendiente.");


            var updatedData = DeliveryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _deliveryRepository.UpdateAsync(existing);
            return DeliveryMapper.ToResponse(updated);
        }


        public async Task<DeliveryResponse> DeleteDeliveryAsync(DeleteRequest request)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la entrega seleccionada.");

            delivery.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _deliveryRepository.DeleteAsync(delivery);

            return DeliveryMapper.ToResponse(delivery);
        }

        public async Task<DeliveryResponse> GetDeliveryByIdAsync(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la entrega seleccionada.");

            return DeliveryMapper.ToResponse(delivery);
        }

        public async Task<List<DeliveryResponse>> GetAllDeliveriesAsync(QueryOptions options)
        {
            var deliveries = await _deliveryRepository.GetAllAsync(options);
            return deliveries.Select(d => DeliveryMapper.ToResponse(d)).ToList();
        }
        internal async Task<DeliveryResponse?> ChangeStatusDeliveryAsync(int id, DeliveryChangeStatusRequest request)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró la entrega con el ID {id}.");

            if (delivery.Status != Status.Pending)
                throw new ArgumentException("La entrega no se puede modificar porque no está en estado pendiente.");

            int userId = request.getUserId() ?? 0;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza el cambio de estado no existe.");

            delivery.AuditInfo.SetUpdated(userId, request.AuditLocation);
            delivery.User = user;

            if (request.Status == Status.Confirmed)
            {
                delivery.Confirm();

                int crates = request.Crates;
                if (crates != 0)
                {
                    int clientId = delivery.Client?.Id ?? throw new ArgumentException("No se pudo obtener el idel cliente asociado a la entrega para ajustar los cajones.");
                    Client client = await _clientRepository.GetByIdAsync(clientId) ?? throw new ArgumentException($"no se encontro el cliente de id {clientId}");
                    client.LoanedCrates += crates - delivery.Order.Crates;
                    client.AuditInfo.SetUpdated(userId, request.AuditLocation);
                    _clientRepository.UpdateAsync(client).Wait();

                }

                // Crear el recibo (Receipt) asociado a la entrega confirmada
                PaymentMethod? paymentMethod = request.PaymentMethod;
                decimal amount = request.Amount ?? 0;


                if (paymentMethod != null) { 

                    var receiptAddRequest = new ClientReceiptAddRequest
                    {
                        Date = DateTime.Now,
                        Amount = amount,
                        Notes = $"Recibo generado automáticamente al confirmar la entrega {delivery.Id}",
                        PaymentMethod = paymentMethod ?? PaymentMethod.Cash,
                        ClientId = delivery.Client.Id
                    };
                    receiptAddRequest.setUserId(userId);
                    receiptAddRequest.AuditLocation = request.AuditLocation;

                    _receiptSubSystem.AddReceiptAsync(receiptAddRequest).Wait();
                }
            }
            else if (request.Status == Status.Cancelled)
            {
                delivery.Cancel();

                // Devolver el stock a los productos
                foreach (var item in delivery.ProductItems)
                {
                    await _productRepository.UpdateStockAsync(item.Product.Id, item.Quantity, item.Quantity);
                }
            }
            else
            {
                throw new ArgumentException("El estado de la entrega no es válido para cambiar.");
            }

            delivery = await _deliveryRepository.ChangeStatusDeliveryAsync(delivery);
            return DeliveryMapper.ToResponse(delivery);
        }


        public async Task<List<DeliveryResponse>> GetConfirmedDeliveriesByClientIdAsync(int clientId, QueryOptions? options = null)
        {

            var deliveries = await _deliveryRepository.GetConfirmedByClientIdAsync(clientId, options);
            if(deliveries == null)throw new ArgumentException($"No se encontró las entregas del client con el ID {clientId}.");
            return deliveries.Select(d => DeliveryMapper.ToResponse(d)).ToList();
        }

    }
}
