using BusinessLogic.Dominio;
using BusinessLogic.DTOsClient;
using BusinessLogic.NewFolder;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {
        private readonly IClientRepository _clientRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ZonesSubSystem _zonesSubSystem;
        public ClientsSubSystem(IClientRepository clientRepository, IReceiptRepository receiptRepository, ZonesSubSystem _ZonesSubSystem)
        {
            _clientRepository = clientRepository;
            _receiptRepository = receiptRepository;
            _zonesSubSystem = _ZonesSubSystem;
        }

        public void AddClient(AddClientRequest addClientRequest)
        {

            var newClient = new Client(addClientRequest.Name,addClientRequest.RUT,addClientRequest.RazonSocial,addClientRequest.Address,addClientRequest.MapsAddress,addClientRequest.Schedule,addClientRequest.Phone,addClientRequest.ContactName,addClientRequest.Email,addClientRequest.Observations,addClientRequest.BankAccount,addClientRequest.LoanedCrates,_zonesSubSystem.GetZoneById(addClientRequest.ZoneId));

            newClient.Validate();

            _clientRepository.Add(newClient);
        }


        public void UpdateClient(UpdateClientRequest updateClientRequest)
        {
            var client = _clientRepository.GetById(updateClientRequest.Id);
            

        }






        public void AddReceipt(Receipt receipt)
        {
            _receiptRepository.Add(receipt);
        }
        public void AsignZone(Zone zone,Client client)
        {

            //Logica para asignar zona a cliente     

        }

    }

}
