using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.Common.Mappers;
using BusinessLogic.DTOs;
using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.Services;

namespace BusinessLogic.SubSystem
{
    public class ZonesSubSystem
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IAzureBlobService _blobService;
        private readonly IClientRepository _clientRepository;
        private readonly IDistributionRepository _distributionRepository;
        private readonly IDiscountRepository _discountRepository;

        public ZonesSubSystem(IZoneRepository zoneRepository,IAzureBlobService azureBlobService, IClientRepository clientRepository, IDistributionRepository distributionRepository, IDiscountRepository discountRepository)
        {
            _zoneRepository = zoneRepository;
            _blobService = azureBlobService;
            _clientRepository = clientRepository;
            _distributionRepository = distributionRepository;
            _discountRepository = discountRepository;
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
            var existingZone = await _zoneRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la zona seleccionada.");

            var existing = await _zoneRepository.GetByNameAsync(request.Name);
            if (existingZone.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe una zona con ese nombre.");

            var updatedData = ZoneMapper.ToUpdatableData(request);
            existingZone.Update(updatedData);

            var updated = await _zoneRepository.UpdateAsync(existingZone);
            return ZoneMapper.ToResponse(updated);
        }

        public async Task<ZoneResponse> DeleteZoneAsync(DeleteRequest request)
        {
            var zone = await _zoneRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la zona seleccionada.");

            var options = new QueryOptions
            {
                Filters = new Dictionary<string, string>
                {
                    { "ZoneId", request.Id.ToString() }
                }
            };
            List<Client> clientsInZone = await _clientRepository.GetAllAsync(options);
            if (clientsInZone.Any())
            {
                throw new InvalidOperationException("No se puede eliminar la zona porque tiene clientes asociados.");
            }
            List<Discount> discounts = await _discountRepository.GetAllAsync(options);
            if (discounts.Any())
            {
                throw new InvalidOperationException("No se puede eliminar la zona porque tiene descuentos asociados.");
            }

            List<Distribution> distributions = await _distributionRepository.GetAllAsync(new QueryOptions());
            if (distributions.Any(d => d.Zones.Any(z => z.Id == request.Id)))
            {
                throw new InvalidOperationException("No se puede eliminar la zona porque tiene repartos asociados.");
            }

            zone.MarkAsDeleted(request.getUserId(), request.AuditLocation);
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
            zone.AuditInfo.SetUpdated(request.getUserId(), request.AuditLocation);

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
