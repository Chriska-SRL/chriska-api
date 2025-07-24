using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs;
using BusinessLogic.Común;

namespace BusinessLogic.SubSystem
{
    public class ZonesSubSystem
    {
        private readonly IZoneRepository _zoneRepository;

        public ZonesSubSystem(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public async Task<ZoneResponse> AddZoneAsync(AddZoneRequest request)
        {
            var newZone = ZoneMapper.ToDomain(request);
            newZone.Validate();

            var added = await _zoneRepository.AddAsync(newZone);
            return ZoneMapper.ToResponse(added);
        }


        public async Task<ZoneResponse> UpdateZoneAsync(UpdateZoneRequest request)
        {
            var existing = await _zoneRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("Zona no encontrada.", nameof(request.Id));

            var updatedData = ZoneMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _zoneRepository.UpdateAsync(existing);
            return ZoneMapper.ToResponse(updated);
        }


        public async Task<ZoneResponse> DeleteZoneAsync(DeleteRequest request)
        {
            var zone = await _zoneRepository.GetByIdAsync(request.Id)
                ?? throw new InvalidOperationException("Zona no encontrada.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            zone.SetDeletedAudit(auditInfo);

            await _zoneRepository.DeleteAsync(zone);
            return ZoneMapper.ToResponse(zone);
        }
        public async Task<ZoneResponse> GetZoneByIdAsync(int id)
        {
            var zone = await _zoneRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Zona no encontrada.");

            return ZoneMapper.ToResponse(zone);
        }

        public async Task<List<ZoneResponse>> GetAllZonesAsync(QueryOptions options)
        {
            var zones = await _zoneRepository.GetAllAsync(options);
            return zones.Select(ZoneMapper.ToResponse).ToList();
        }
    }
}
