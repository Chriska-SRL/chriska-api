using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.Común.Mappers;

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

        public void AddClient(AddClientRequest addClientRequest)
        {
            Client client = ClientMapper.ToDomain(addClientRequest);
            client.Validate();
            _clientRepository.Add(client);
        }

        public void UpdateClient(UpdateClientRequest updateClientRequest)
        {
            Client existingClient = _clientRepository.GetById(updateClientRequest.Id);
            if (existingClient == null) throw new Exception("No se encontro el cliente");
            existingClient.Update(ClientMapper.ToDomain(updateClientRequest));
            _clientRepository.Update(existingClient);
        }

        public void DeleteClient(DeleteClientRequest deleteClientRequest)
        {
            var client = _clientRepository.GetById(deleteClientRequest.Id);
            if (client == null) throw new Exception("No se encontro el cliente");
            _clientRepository.Delete(deleteClientRequest.Id);
        }

        public List<ClientResponse> GetAllClients()
        {
            List<Client> listClient = _clientRepository.GetAll();
            if (!listClient.Any()) throw new Exception("No se encontraron clientes");
            return listClient.Select(ClientMapper.ToResponse).ToList();
        }

        public ClientResponse GetClientById(int id)
        {
            Client client = _clientRepository.GetById(id);
            if (client == null) throw new Exception("No se encontro el cliente");
            ClientResponse clientResponse = ClientMapper.ToResponse(client);
            return clientResponse;
        }
        
        public void AddReceipt(AddReceiptRequest receipt)
        {
            Receipt newReceipt = ReceiptMapper.ToDomain(receipt);
            newReceipt.Validate();
            _receiptRepository.Add(newReceipt);
        }

        public void UpdateReceipt(UpdateReceiptRequest receipt) {

            Receipt existingReceipt = _receiptRepository.GetById(receipt.Id);
            if (existingReceipt == null) throw new Exception("No se encontro el recibo");
            existingReceipt.Update(ReceiptMapper.ToDomain(receipt));
            _receiptRepository.Update(existingReceipt);
        }
        public void DeleteReceipt(DeleteReceiptRequest receipt)
        {
            Receipt existingReceipt = _receiptRepository.GetById(receipt.Id);
            if (existingReceipt == null) throw new Exception("No se encontro el recibo");
            _receiptRepository.Delete(receipt.Id);
        }
        public List<ReceiptResponse> GetAllReceipts()
        {
            List<Receipt> listReceipt = _receiptRepository.GetAll();
            if (!listReceipt.Any()) throw new Exception("No se encontraron recibos");
            return listReceipt.Select(ReceiptMapper.ToResponse).ToList();
        }
        public ReceiptResponse GetReceiptById(int id)
        {
            Receipt receipt = _receiptRepository.GetById(id);
            if (receipt == null) throw new Exception("No se encontro el recibo");
            ReceiptResponse receiptResponse = ReceiptMapper.ToResponse(receipt);
            return receiptResponse;
        }
    }
}
