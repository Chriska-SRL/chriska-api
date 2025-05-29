using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {
        private readonly IClientRepository _clientRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IZoneRepository _zoneRepository;

        private readonly ZonesSubSystem zonesSubSystem;
        private readonly ClientsSubSystem clientsSubSystem;
        public ClientsSubSystem(IClientRepository clientRepository, IReceiptRepository receiptRepository, IZoneRepository zoneRepository, ZonesSubSystem _ZonesSubSystem)
        {
            _clientRepository = clientRepository;
            _receiptRepository = receiptRepository;
            _zoneRepository = zoneRepository;

            zonesSubSystem = _ZonesSubSystem;
        }

        public void AddClient(AddClientRequest addClientRequest)
        {
            var newClient = new Client(addClientRequest.Name,addClientRequest.RUT,addClientRequest.RazonSocial,addClientRequest.Address,addClientRequest.MapsAddress,addClientRequest.Schedule,addClientRequest.Phone,addClientRequest.ContactName,addClientRequest.Email,addClientRequest.Observations,addClientRequest.BankAccount,addClientRequest.LoanedCrates,_zoneRepository.GetById(addClientRequest.ZoneId));
            newClient.Validate();
            _clientRepository.Add(newClient);
        }

        public void UpdateClient(UpdateClientRequest updateClientRequest)
        {
            var client = _clientRepository.GetById(updateClientRequest.Id);
            if(client == null) throw new Exception("No se encontro el cliente");
            client.Update(updateClientRequest.Name, updateClientRequest.RUT, updateClientRequest.RazonSocial, updateClientRequest.Address, updateClientRequest.MapsAddress, updateClientRequest.Schedule, updateClientRequest.Phone, updateClientRequest.ContactName, updateClientRequest.Email, updateClientRequest.Observations, updateClientRequest.BankAccount, updateClientRequest.LoanedCrates, _zoneRepository.GetById(updateClientRequest.ZoneId));
            _clientRepository.Update(client);
        }

        public void DeleteClient(DeleteClientRequest deleteClientRequest)
        {
            var client = _clientRepository.GetById(deleteClientRequest.Id);
            if (client == null) throw new Exception("No se encontro el cliente");
            _clientRepository.Delete(deleteClientRequest.Id);
        }

        public List<ClientResponse> GetAllClients()
        {
            var list = _clientRepository.GetAll();
            if (list == null) throw new Exception("No se encontraron clientes");
            var listClientResponse = new List<ClientResponse>();
            foreach (var client in list)
            {
                var clientResponse = new ClientResponse
                {
                    Name = client.Name,
                    RUT = client.RUT,
                    RazonSocial = client.RazonSocial,
                    Address = client.Address,
                    MapsAddress = client.MapsAddress,
                    Schedule = client.Schedule,
                    Phone = client.Phone,
                    ContactName = client.ContactName,
                    Email = client.Email,
                    Observation = client.Observation,
                    BankAccount = client.BankAccount,
                    LoanedCrates = client.LoanedCrates,
                    zone= zonesSubSystem.GetZoneById(client.Zone.Id)
                };
                listClientResponse.Add(clientResponse);
            }
            return listClientResponse;
        }

        public ClientResponse GetClientById(int id)
        {
            var client = _clientRepository.GetById(id);
            if (client == null) throw new Exception("No se encontro el cliente");
            var clientResponse = new ClientResponse
            {
                Name = client.Name,
                RUT = client.RUT,
                RazonSocial = client.RazonSocial,
                Address = client.Address,
                MapsAddress = client.MapsAddress,
                Schedule = client.Schedule,
                Phone = client.Phone,
                ContactName = client.ContactName,
                Email = client.Email,
                Observation = client.Observation,
                BankAccount = client.BankAccount,
                LoanedCrates = client.LoanedCrates,
                zone = zonesSubSystem.GetZoneById(client.Zone.Id)
            };
            return clientResponse;
        }
        
        public void AddReceipt(AddReceiptRequest receipt)
        {
            var newReceipt = new Receipt(receipt.Date, receipt.Amount, receipt.PaymentMethod, receipt.Notes, _clientRepository.GetById(receipt.ClientId));
            newReceipt.Validate();
            _receiptRepository.Add(newReceipt);
        }

        public void UpdateReceipt(UpdateReceiptRequest receipt)
        {
            var existingReceipt = _receiptRepository.GetById(receipt.Id);
            if (existingReceipt == null) throw new Exception("No se encontro el recibo");
            existingReceipt.Update(receipt.Amount, receipt.PaymentMethod, receipt.Notes, _clientRepository.GetById(receipt.ClientId));
            _receiptRepository.Update(existingReceipt);
        }
        
        public ReceiptResponse GetReceiptById(int id)
        {
            var receipt = _receiptRepository.GetById(id);
            if (receipt == null) throw new Exception("No se encontro el recibo");
            var receiptResponse = new ReceiptResponse
            {
                Date = receipt.Date,
                Amount = receipt.Amount,
                PaymentMethod = receipt.PaymentMethod,
                Notes = receipt.Notes,
                Client = clientsSubSystem.GetClientById(receipt.Client.Id)
            };
            return receiptResponse;
        }


    }
}
