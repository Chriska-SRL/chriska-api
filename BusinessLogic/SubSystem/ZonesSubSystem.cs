using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.Común.Mappers;

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
            Zone newZone = ZoneMapper.toDomain(zone);
            newZone.Validate();
            _zoneRepository.Add(newZone);
        }

        public void UpdateZone(UpdateZoneRequest zone)
        {
            Zone existingZone = _zoneRepository.GetById(zone.Id);
            if (existingZone == null) throw new Exception("No se encontro la zona");
            existingZone.Update(ZoneMapper.toDomain(zone));
            _zoneRepository.Update(existingZone);
        }

        public void DeleteZone(DeleteZoneRequest zone)
        {
            Zone existingZone = _zoneRepository.GetById(zone.Id);
            if (existingZone == null) throw new Exception("No se encontro la zona");
            _zoneRepository.Delete(existingZone.Id);
        }

        public ZoneResponse GetZoneById(int id)
        {
            Zone zone = _zoneRepository.GetById(id);
            if (zone == null) throw new Exception("No se encontro la zona");
            return ZoneMapper.toResponse(zone);
        }

        public List<ZoneResponse> GetAllZones()
        {
            List<Zone> zones = _zoneRepository.GetAll();
            if (zones == null || zones.Count == 0) throw new Exception("No se encontraron zonas");
            return zones.Select(ZoneMapper.toResponse).ToList();
        }
    }
}
