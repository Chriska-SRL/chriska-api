using BusinessLogic.Dominio;
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
        private readonly IZoneRepository _zoneRepository;
        public ClientsSubSystem(IClientRepository clientRepository, IReceiptRepository receiptRepository, IZoneRepository zoneRepository)
        {
            _clientRepository = clientRepository;
            _receiptRepository = receiptRepository;
            _zoneRepository = zoneRepository;
        }

        public void AddClient(Client client)
        {
            _clientRepository.Add(client);
        }
        public void AddReceipt(Receipt receipt)
        {
            _receiptRepository.Add(receipt);
        }
        public void AsignZone(Zone zone,Client client)
        {

            //_zoneRepository.Add(zone);
        }
    }

}
