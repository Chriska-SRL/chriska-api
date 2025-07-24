using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;
using BusinessLogic.Común;

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
            var zone = await _zoneRepository.GetByIdAsync(request.ZoneId)
                        ?? throw new ArgumentException("Zona no encontrada.", nameof(request.ZoneId));

            if (await _clientRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo RUT.", nameof(request.RUT));

            if (await _clientRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo nombre.", nameof(request.Name));

            var newClient = ClientMapper.ToDomain(request);
            newClient.Zone = zone;
            newClient.Validate();

            var added = await _clientRepository.AddAsync(newClient);
            return ClientMapper.ToResponse(added);
        }

        public async Task<ClientResponse> UpdateClientAsync(UpdateClientRequest request)
        {
            var zone = await _zoneRepository.GetByIdAsync(request.ZoneId)
                        ?? throw new ArgumentException("Zona no encontrada.", nameof(request.ZoneId));

            var existing = await _clientRepository.GetByIdAsync(request.Id)
                           ?? throw new InvalidOperationException("Cliente no encontrado.");

            if (existing.RUT != request.RUT && await _clientRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo RUT.", nameof(request.RUT));

            if (existing.Name != request.Name && await _clientRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo nombre.", nameof(request.Name));

            var updatedData = ClientMapper.ToUpdatableData(request);
            updatedData.Zone = zone;
            existing.Update(updatedData);

            var updated = await _clientRepository.UpdateAsync(existing);
            return ClientMapper.ToResponse(updated);
        }

        public async Task<ClientResponse> DeleteClientAsync(DeleteClientRequest request)
        {
            var client = await _clientRepository.GetByIdAsync(request.Id)
                          ?? throw new InvalidOperationException("Cliente no encontrado.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            client.SetDeletedAudit(auditInfo);

            await _clientRepository.DeleteAsync(client);
            return ClientMapper.ToResponse(client);
        }
        public async Task<ClientResponse> GetClientByIdAsync(int id)
        {
            Client client = await _clientRepository.GetByIdAsync(id)
                               ?? throw new InvalidOperationException("Cliente no encontrado.");

            return ClientMapper.ToResponse(client);
        }

        public async Task<List<ClientResponse>> GetAllClientsAsync(QueryOptions options)
        {
            var clients = await _clientRepository.GetAllAsync(options);
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

        public async Task<ReceiptResponse> DeleteReceiptAsync(DeleteReceiptRequest request)
        {
            var receipt = await _receiptRepository.GetByIdAsync(request.Id)
                            ?? throw new InvalidOperationException("Recibo no encontrado.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            receipt.SetDeletedAudit(auditInfo);

            await _receiptRepository.DeleteAsync(receipt);
            return ReceiptMapper.ToResponse(receipt);
        }


        public async Task<ReceiptResponse> GetReceiptByIdAsync(int id)
        {
            Receipt receipt = await _receiptRepository.GetByIdAsync(id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");

            return ReceiptMapper.ToResponse(receipt);
        }

        public async Task<List<ReceiptResponse>> GetAllReceiptsAsync(QueryOptions options)
        {
            var receipts = await _receiptRepository.GetAllAsync(options);
            return receipts.Select(ReceiptMapper.ToResponse).ToList();
        }
    }
}
