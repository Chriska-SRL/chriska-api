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
        private readonly IZoneRepository _zoneRepository;

        private readonly ZonesSubSystem _zonesSubSystem;

        public ClientsSubSystem(IClientRepository clientRepository, IReceiptRepository receiptRepository, IZoneRepository zoneRepository, ZonesSubSystem _ZonesSubSystem)
        {
            _clientRepository = clientRepository;
            _receiptRepository = receiptRepository;
            _zoneRepository = zoneRepository;

            _zonesSubSystem = _ZonesSubSystem;
        }

        public void AddClient(AddClientRequest addClientRequest)
        {
            Client client = ClientMapper.toDomain(addClientRequest);
            _clientRepository.Add(client);
        }

        public void UpdateClient(UpdateClientRequest updateClientRequest)
        {
            Client existingClient = _clientRepository.GetById(updateClientRequest.Id);
            if (existingClient == null) throw new Exception("No se encontro el cliente");
            existingClient.Update(ClientMapper.toDomain(updateClientRequest));
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
            if (listClient == null) throw new Exception("No se encontraron clientes");
            List<ClientResponse> listClientResponse = new List<ClientResponse>();
            foreach (Client client in listClient)
            {
                ClientResponse clientResponse = ClientMapper.toResponse(client);
                listClientResponse.Add(clientResponse);
            }
            return listClientResponse;
        }

        public ClientResponse GetClientById(int id)
        {
            Client client = _clientRepository.GetById(id);
            if (client == null) throw new Exception("No se encontro el cliente");
            ClientResponse clientResponse = ClientMapper.toResponse(client);
            return clientResponse;
        }
        
        public void AddReceipt(AddReceiptRequest receipt)
        {
            Receipt newReceipt = ReceiptMapper.toDomain(receipt);
            _receiptRepository.Add(newReceipt);
        }

        public void UpdateReceipt(UpdateReceiptRequest receipt) {

            Receipt existingReceipt = _receiptRepository.GetById(receipt.Id);
            if (existingReceipt == null) throw new Exception("No se encontro el recibo");
            existingReceipt.Update(ReceiptMapper.toDomain(receipt));
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
            if (listReceipt == null) throw new Exception("No se encontraron recibos");
            List<ReceiptResponse> listReceiptResponse = new List<ReceiptResponse>();
            foreach (Receipt receipt in listReceipt)
            {
                ReceiptResponse receiptResponse = ReceiptMapper.toResponse(receipt);
                listReceiptResponse.Add(receiptResponse);
            }
            return listReceiptResponse;
        }
        public ReceiptResponse GetReceiptById(int id)
        {
            Receipt receipt = _receiptRepository.GetById(id);
            if (receipt == null) throw new Exception("No se encontro el recibo");
            ReceiptResponse receiptResponse = ReceiptMapper.toResponse(receipt);
            return receiptResponse;
        }
    }
}
