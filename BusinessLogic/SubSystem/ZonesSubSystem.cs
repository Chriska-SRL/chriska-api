using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.SubSystem
{
    public class ZonesSubSystem
    {
        private readonly IZoneRepository _zoneRepository;

        public ZonesSubSystem(IZoneRepository zoneRepository, IClientRepository clientRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public void AddZone(AddZoneRequest zone)
        {
            //var newZone = new Zone(zone.Name, zone.Description);
            //newZone.Validate();
            //_zoneRepository.Add(newZone);
        }

        public void UpdateZone(UpdateZoneRequest zone)
        {
            //var existingZone = _zoneRepository.GetById(zone.Id);
            //if (existingZone == null) throw new Exception("No se encontro la zona");
            //existingZone.Update(zone.Name, zone.Description);
            //existingZone.Validate();
            //_zoneRepository.Update(existingZone);
        }

        public void DeleteZone(DeleteZoneRequest zone)
        {
            var existingZone = _zoneRepository.GetById(zone.Id);
            if (existingZone == null) throw new Exception("No se encontro la zona");
            _zoneRepository.Delete(existingZone.Id);
        }

        public ZoneResponse GetZoneById(int id)
        {
            var zone = _zoneRepository.GetById(id);
            if (zone == null) throw new Exception("No se encontro la zona");
            var zoneResponse = new ZoneResponse
            {
                Name = zone.Name,
                Description = zone.Description
            };
            return zoneResponse;

        }

        public List<ZoneResponse> GetAllZones()
        {
            var zones = _zoneRepository.GetAll();
            if (zones == null || !zones.Any()) throw new Exception("No hay zonas registradas");
            return zones.Select(z => new ZoneResponse
            {
                Name = z.Name,
                Description = z.Description
            }).ToList();
        }
    }
}
