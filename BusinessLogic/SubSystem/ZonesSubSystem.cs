using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs;
using BusinessLogic.Común;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.Services;

namespace BusinessLogic.SubSystem
{
    public class ZonesSubSystem
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IAzureBlobService _blobService;

        public ZonesSubSystem(IZoneRepository zoneRepository,IAzureBlobService azureBlobService)
        {
            _zoneRepository = zoneRepository;
            _blobService = azureBlobService;
        }

        public async Task<ZoneResponse> AddZoneAsync(AddZoneRequest request)
        {
            var newZone = ZoneMapper.ToDomain(request);
            newZone.Validate();

            var existing = await _zoneRepository.GetByNameAsync(newZone.Name);
            if (existing != null)
                throw new ArgumentException("Ya existe una zona con ese nombre.");

            var added = await _zoneRepository.AddAsync(newZone);
            return ZoneMapper.ToResponse(added);
        }

        public async Task<ZoneResponse> UpdateZoneAsync(UpdateZoneRequest request)
        {
            var existingBrand = await _zoneRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la zona seleccionada.");

            var existing = await _zoneRepository.GetByNameAsync(request.Name);
            if (existingBrand.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe una zona con ese nombre.");

            var updatedData = ZoneMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _zoneRepository.UpdateAsync(existing);
            return ZoneMapper.ToResponse(updated);
        }

        public async Task<ZoneResponse> DeleteZoneAsync(DeleteRequest request)
        {
            var zone = await _zoneRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la zona seleccionada.");

            zone.MarkAsDeleted(request.getUserId(), request.Location);
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

        public async Task<string> UploadZoneImageAsync(AddImageRequest request)
        {
            Zone zone = await _zoneRepository.GetByIdAsync(request.EntityId)
                ?? throw new ArgumentException("No se encontró la zona seleccionada.");
            zone.AuditInfo.SetUpdated(request.getUserId(), request.Location);

            var url = await _blobService.UploadFileAsync(request.File, "zones", $"zone{zone.Id}");
            return await _zoneRepository.UpdateImageUrlAsync(zone, url);
        }

        public async Task DeleteZoneImageAsync(int zoneId)
        {
            Zone zone = await _zoneRepository.GetByIdAsync(zoneId)
                ?? throw new ArgumentException("No se encontró la zona seleccionada.");

            await _blobService.DeleteFileAsync(zone.ImageUrl, "zones");
            await _zoneRepository.UpdateImageUrlAsync(zone, "");
        }
    }
}
