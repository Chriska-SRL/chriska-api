using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public  class ZonesSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private IZoneRepository _zoneRepository;
        private IClientRepository _clientRepository;
        public ZonesSubSystem(IZoneRepository zoneRepository, IClientRepository clientRepository)
        {
            _zoneRepository = zoneRepository;
            _clientRepository = clientRepository;
        }
        public void AddZone(Zone zone)
        {
            _zoneRepository.Add(zone);
        }
        public void AddClient(Client client)
        {
            _clientRepository.Add(client);
        }

    }
}
