using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ZonesSubSystem
    {
        private readonly IZoneRepository _zoneRepository;

        public ZonesSubSystem(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public ZoneResponse AddZone(AddZoneRequest request)
        {
            Zone newZone = ZoneMapper.ToDomain(request);
            newZone.Validate();

            Zone added = _zoneRepository.Add(newZone);
            return ZoneMapper.ToResponse(added);
        }

        public ZoneResponse UpdateZone(UpdateZoneRequest request)
        {
            Zone existingZone = _zoneRepository.GetById(request.Id)
                                 ?? throw new InvalidOperationException("Zona no encontrada.");

            Zone.UpdatableData updatedData = ZoneMapper.ToUpdatableData(request);
            existingZone.Update(updatedData);

            Zone updated = _zoneRepository.Update(existingZone);
            return ZoneMapper.ToResponse(updated);
        }

        public ZoneResponse DeleteZone(DeleteZoneRequest request)
        {
            Zone deleted = _zoneRepository.Delete(request.Id)
                            ?? throw new InvalidOperationException("Zona no encontrada.");

            return ZoneMapper.ToResponse(deleted);
        }

        public ZoneResponse GetZoneById(int id)
        {
            Zone zone = _zoneRepository.GetById(id)
                         ?? throw new InvalidOperationException("Zona no encontrada.");

            return ZoneMapper.ToResponse(zone);
        }

        public List<ZoneResponse> GetAllZones()
        {
            List<Zone> zones = _zoneRepository.GetAll();
            return zones.Select(ZoneMapper.ToResponse).ToList();
        }
    }
}
