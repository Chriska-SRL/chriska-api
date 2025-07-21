using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {
        private readonly IClientRepository _clientRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IZoneRepository _zoneRepository;

        public ClientsSubSystem(IClientRepository clientRepository, IReceiptRepository receiptRepository, IZoneRepository zoneRepository)
        {
            _clientRepository = clientRepository;
            _receiptRepository = receiptRepository;
            _zoneRepository = zoneRepository;
        }

        // Clientes

        public async Task<ClientResponse> AddClientAsync(AddClientRequest request)
        {
            Zone zone = await _zoneRepository.GetByIdAsync(request.ZoneId)
                          ?? throw new ArgumentException("Zona no encontrada.", nameof(request.ZoneId));

            if (await _clientRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo RUT.", nameof(request.RUT));

            if (await _clientRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo nombre.", nameof(request.Name));

            Client newclient = ClientMapper.ToDomain(request);
            newclient.Zone = zone;
            newclient.Validate();

            Client added = await _clientRepository.AddAsync(newclient);
            return ClientMapper.ToResponse(added);
        }

        public async Task<ClientResponse> UpdateClientAsync(UpdateClientRequest request)
        {
            Zone zone = await _zoneRepository.GetByIdAsync(request.ZoneId)
                          ?? throw new ArgumentException("Zona no encontrada.", nameof(request.ZoneId));

            Client existing = await _clientRepository.GetByIdAsync(request.Id)
                               ?? throw new InvalidOperationException("Cliente no encontrado.");

            if (existing.RUT != request.RUT && await _clientRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo RUT.", nameof(request.RUT));

            if (existing.Name != request.Name && await _clientRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo nombre.", nameof(request.Name));

            Client.UpdatableData updatedData = ClientMapper.ToUpdatableData(request);
            updatedData.Zone = zone;
            existing.Update(updatedData);

            Client updated = await _clientRepository.UpdateAsync(existing);
            return ClientMapper.ToResponse(updated);
        }

        public async Task<ClientResponse> DeleteClientAsync(int id)
        {
            Client deleted = await _clientRepository.DeleteAsync(id)
                               ?? throw new InvalidOperationException("Cliente no encontrado.");

            return ClientMapper.ToResponse(deleted);
        }

        public async Task<ClientResponse> GetClientByIdAsync(int id)
        {
            Client client = await _clientRepository.GetByIdAsync(id)
                               ?? throw new InvalidOperationException("Cliente no encontrado.");

            return ClientMapper.ToResponse(client);
        }

        public async Task<List<ClientResponse>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(ClientMapper.ToResponse).ToList();
        }

        // Recibos

        public async Task<ReceiptResponse> AddReceiptAsync(AddReceiptRequest request)
        {
            Client client = await _clientRepository.GetByIdAsync(request.ClientId)
                             ?? throw new ArgumentException("Cliente no encontrado.", nameof(request.ClientId));

            Receipt receipt = ReceiptMapper.ToDomain(request);
            receipt.Client = client;
            receipt.Validate();

            Receipt added = await _receiptRepository.AddAsync(receipt);
            return ReceiptMapper.ToResponse(added);
        }

        public async Task<ReceiptResponse> UpdateReceiptAsync(UpdateReceiptRequest request)
        {
            Receipt existing = await _receiptRepository.GetByIdAsync(request.Id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");

            Client client = await _clientRepository.GetByIdAsync(request.ClientId)
                            ?? throw new ArgumentException("Cliente no encontrado.", nameof(request.ClientId));

            Receipt.UpdatableData updatedData = ReceiptMapper.ToUpdatableData(request);
            updatedData.Client = client;
            existing.Update(updatedData);

            Receipt updated = await _receiptRepository.UpdateAsync(existing);
            return ReceiptMapper.ToResponse(updated);
        }

        public async Task<ReceiptResponse> DeleteReceiptAsync(int id)
        {
            Receipt deleted = await _receiptRepository.DeleteAsync(id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");

            return ReceiptMapper.ToResponse(deleted);
        }

        public async Task<ReceiptResponse> GetReceiptByIdAsync(int id)
        {
            Receipt receipt = await _receiptRepository.GetByIdAsync(id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");

            return ReceiptMapper.ToResponse(receipt);
        }

        public async Task<List<ReceiptResponse>> GetAllReceiptsAsync()
        {
            var receipts = await _receiptRepository.GetAllAsync();
            return receipts.Select(ReceiptMapper.ToResponse).ToList();
        }
    }
}
