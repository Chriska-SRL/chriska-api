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

        private readonly IZoneRepository _zoneRepository;
     
        public ZonesSubSystem(IZoneRepository zoneRepository, IClientRepository clientRepository)
        {
            _zoneRepository = zoneRepository;
            
        }
        public void AddZone(Zone zone)
        {
            _zoneRepository.Add(zone);
        }

        public void UpdateZone(Zone zone)
        {
            _zoneRepository.Update(zone);
        }
        public void DeleteZone(Zone zone)
        {
            _zoneRepository.Delete(zone);
        }
        public Zone GetZoneById(int id)
        {
            return _zoneRepository.GetById(id);
        }


    }
}
