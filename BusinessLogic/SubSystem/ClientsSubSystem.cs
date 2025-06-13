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

        public ClientsSubSystem(IClientRepository clientRepository, IReceiptRepository receiptRepository)
        {
            _clientRepository = clientRepository;
            _receiptRepository = receiptRepository;
        }

        // Clientes

        public ClientResponse AddClient(AddClientRequest request)
        {
            Client client = ClientMapper.ToDomain(request);
            client.Validate();

            Client added = _clientRepository.Add(client);
            return ClientMapper.ToResponse(added);
        }

        public ClientResponse UpdateClient(UpdateClientRequest request)
        {
            Client existing = _clientRepository.GetById(request.Id)
                              ?? throw new InvalidOperationException("Cliente no encontrado.");

            Client.UpdatableData updatedData = ClientMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Client updated = _clientRepository.Update(existing);
            return ClientMapper.ToResponse(updated);
        }

        public ClientResponse DeleteClient(DeleteClientRequest request)
        {
            Client deleted = _clientRepository.Delete(request.Id)
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
            Receipt receipt = ReceiptMapper.ToDomain(request);
            receipt.Validate();

            Receipt added = _receiptRepository.Add(receipt);
            return ReceiptMapper.ToResponse(added);
        }

        public ReceiptResponse UpdateReceipt(UpdateReceiptRequest request)
        {
            Receipt existing = _receiptRepository.GetById(request.Id)
                               ?? throw new InvalidOperationException("Recibo no encontrado.");

            Receipt.UpdatableData updatedData = ReceiptMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Receipt updated = _receiptRepository.Update(existing);
            return ReceiptMapper.ToResponse(updated);
        }

        public ReceiptResponse DeleteReceipt(DeleteReceiptRequest request)
        {
            Receipt deleted = _receiptRepository.Delete(request.Id)
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
