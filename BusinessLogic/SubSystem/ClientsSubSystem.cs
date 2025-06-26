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

        public ClientResponse AddClient(AddClientRequest request)
        {
            
                Zone zone = _zoneRepository.GetById(request.ZoneId)
                               ?? throw new ArgumentException("Zona no encontrada.", nameof(request.ZoneId));

                if (_clientRepository.GetByRUT(request.RUT) != null)
                    throw new ArgumentException("Ya existe un cliente con el mismo RUT.", nameof(request.RUT));

                if (_clientRepository.GetByName(request.Name) != null)
                    throw new ArgumentException("Ya existe un cliente con el mismo nombre.", nameof(request.Name));

                Client newclient = ClientMapper.ToDomain(request);
                newclient.Zone = zone;
                newclient.Validate();

                Client added = _clientRepository.Add(newclient);
                return ClientMapper.ToResponse(added);
           
        }

        public ClientResponse UpdateClient(UpdateClientRequest request)
        {

            Zone zone = _zoneRepository.GetById(request.ZoneId)
                                         ?? throw new ArgumentException("Zona no encontrada.", nameof(request.ZoneId));
     
            Client existing = _clientRepository.GetById(request.Id)
                              ?? throw new InvalidOperationException("Cliente no encontrado.");

            if (existing.RUT != request.RUT && _clientRepository.GetByRUT(request.RUT) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo RUT.", nameof(request.RUT));

            if (existing.Name != request.Name && _clientRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un cliente con el mismo nombre.", nameof(request.Name));


            Client.UpdatableData updatedData = ClientMapper.ToUpdatableData(request);
            updatedData.Zone = zone;
            existing.Update(updatedData);

            Client updated = _clientRepository.Update(existing);
            return ClientMapper.ToResponse(updated);
        }

        public ClientResponse DeleteClient(int id)
        {
            Client deleted = _clientRepository.Delete(id)
                              ?? throw new InvalidOperationException("Cliente no encontrado.");

            return ClientMapper.ToResponse(deleted);
        }

        public ClientResponse GetClientById(int id)
        {
            Client client = _clientRepository.GetById(id)
                              ?? throw new InvalidOperationException("Cliente no encontrado.");

            return ClientMapper.ToResponse(client);
        }

        public List<ClientResponse> GetAllClients()
        {
            return _clientRepository.GetAll()
                                    .Select(ClientMapper.ToResponse)
                                    .ToList();
        }

        // Recibos

        public ReceiptResponse AddReceipt(AddReceiptRequest request)
        {
            Client client = _clientRepository.GetById(request.ClientId)
                             ?? throw new ArgumentException("Cliente no encontrado.", nameof(request.ClientId));
            Receipt receipt = ReceiptMapper.ToDomain(request);
            receipt.Client = client;
            receipt.Validate();

            Receipt added = _receiptRepository.Add(receipt);
            return ReceiptMapper.ToResponse(added);
        }

        public ReceiptResponse UpdateReceipt(UpdateReceiptRequest request)
        {
            Receipt existing = _receiptRepository.GetById(request.Id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");
            Client client = _clientRepository.GetById(request.ClientId)
                            ?? throw new ArgumentException("Cliente no encontrado.", nameof(request.ClientId));

            Receipt.UpdatableData updatedData = ReceiptMapper.ToUpdatableData(request);
            updatedData.Client = client;
            existing.Update(updatedData);

            Receipt updated = _receiptRepository.Update(existing);
            return ReceiptMapper.ToResponse(updated);
        }

        public ReceiptResponse DeleteReceipt(int id)
        {
            Receipt deleted = _receiptRepository.Delete(id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");

            return ReceiptMapper.ToResponse(deleted);
        }

        public ReceiptResponse GetReceiptById(int id)
        {
            Receipt receipt = _receiptRepository.GetById(id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");
            return ReceiptMapper.ToResponse(receipt);
        }

        public List<ReceiptResponse> GetAllReceipts()
        {
            return _receiptRepository.GetAll()
                                     .Select(ReceiptMapper.ToResponse)
                                     .ToList();
        }
    }
}
