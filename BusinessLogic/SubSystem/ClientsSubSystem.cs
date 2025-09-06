using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Common.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {
        private readonly IClientRepository _clientRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;

        public ClientsSubSystem(IClientRepository clientRepository, IZoneRepository zoneRepository, IDeliveryRepository deliveryRepository, IDiscountRepository discountRepository, IOrderRepository orderRepository, IOrderRequestRepository orderRequestRepository)
        {
            _clientRepository = clientRepository;
            _zoneRepository = zoneRepository;
            _deliveryRepository = deliveryRepository;
            _discountRepository = discountRepository;
            _orderRepository = orderRepository;
            _orderRequestRepository = orderRequestRepository;
        }

        public async Task<ClientResponse> AddClientAsync(AddClientRequest request)
        {
            var newClient = ClientMapper.ToDomain(request);
            newClient.Validate();

            if (await _clientRepository.GetByRUTAsync(newClient.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con ese RUT.", nameof(newClient.RUT));

            if (await _clientRepository.GetByNameAsync(newClient.Name) != null)
                throw new ArgumentException("Ya existe un cliente con ese nombre.", nameof(newClient.Name));

            var zone = await _zoneRepository.GetByIdAsync(request.ZoneId)
                ?? throw new ArgumentException("La zona especificada no existe.", nameof(request.ZoneId));

            Client added = await _clientRepository.AddAsync(newClient);
            added.Zone = zone;
            return ClientMapper.ToResponse(added);
        }

        public async Task<ClientResponse> UpdateClientAsync(UpdateClientRequest request)
        {
            var existingClient = await _clientRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el cliente seleccionado.", nameof(request.Id));

            if (existingClient.RUT != request.RUT && await _clientRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con ese RUT.", nameof(request.RUT));

            var zone = await _zoneRepository.GetByIdAsync(request.ZoneId ?? existingClient.Zone.Id)
                ?? throw new ArgumentException("La zona especificada no existe.", nameof(request.ZoneId));

            var updatedData = ClientMapper.ToUpdatableData(request, zone);
            existingClient.Update(updatedData);

            var updated = await _clientRepository.UpdateAsync(existingClient);
            return ClientMapper.ToResponse(updated);
        }

        public async Task DeleteClientAsync(DeleteRequest request)
        {
            var client = await _clientRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el cliente seleccionado.", nameof(request.Id));

            var options = new QueryOptions
            {
                Filters = new Dictionary<string, string>
                {
                    { "ClientId", request.Id.ToString() }
                }
            };

            List<Discount> discounts = await _discountRepository.GetAllAsync(new QueryOptions());
            if (discounts.Any(d => d.Clients.Any(c => c.Id == request.Id)))
            {
                throw new InvalidOperationException("No se puede eliminar la zona porque tiene repartos asociados.");
            }
            List<OrderRequest> orderRequests = await _orderRequestRepository.GetAllAsync(options);
            if (orderRequests.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene pedidos asociados");
            }
            List<Order> order = await _orderRepository.GetAllAsync(options);
            if (order.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene ordenes asociados");
            }
            List<Delivery> deliveries = await _deliveryRepository.GetAllAsync(options);
            if (deliveries.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene entregas asociadas");
            }

            client.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _clientRepository.DeleteAsync(client);
        }

        public async Task<ClientResponse> GetClientByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el cliente seleccionado.", nameof(id));

            return ClientMapper.ToResponse(client);
        }

        public async Task<List<ClientResponse>> GetAllClientsAsync(QueryOptions options)
        {
            var clients = await _clientRepository.GetAllAsync(options);
            return clients.Select(ClientMapper.ToResponse).ToList();
        }
    }
}
