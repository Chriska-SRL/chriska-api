using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {
        private readonly IClientRepository _clientRepository;
        private readonly IZoneRepository _zoneRepository;

        public ClientsSubSystem(IClientRepository clientRepository, IZoneRepository zoneRepository)
        {
            _clientRepository = clientRepository;
            _zoneRepository = zoneRepository;
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

            client.MarkAsDeleted(request.getUserId(), request.Location);
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
