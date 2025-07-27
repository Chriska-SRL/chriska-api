using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.Común.Mappers
{
    public static class ZoneMapper
    {
        public static Zone ToDomain(AddZoneRequest request)
        {
            var zone = new Zone(
                name: request.Name,
                description: request.Description,
                deliveryDays: new List<Day>(),
                requestDays: new List<Day>()
            );

            zone.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return zone;
        }

        public static Zone.UpdatableData ToUpdatableData(UpdateZoneRequest request)
        {
            return new Zone.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                DeliveryDays = request.DeliveryDays.Select(d => (Day)d).ToList(),
                RequestDays = request.RequestDays.Select(d => (Day)d).ToList(),
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static ZoneResponse ToResponse(Zone zone)
        {
            return new ZoneResponse
            {
                Id = zone.Id,
                Name = zone.Name,
                Description = zone.Description,
                ImageUrl = zone.ImageUrl,
                AuditInfo = AuditMapper.ToResponse(zone.AuditInfo)
            };
        }
    }
}
