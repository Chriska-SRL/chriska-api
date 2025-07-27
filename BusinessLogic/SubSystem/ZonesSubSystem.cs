using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.Repository;

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
            throw new NotImplementedException();
        }

        public async Task<ZoneResponse> UpdateZoneAsync(UpdateZoneRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteZoneAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ZoneResponse> GetZoneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ZoneResponse>> GetAllZonesAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
