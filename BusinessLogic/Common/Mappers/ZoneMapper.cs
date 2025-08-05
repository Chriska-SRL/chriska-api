using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.Common.Mappers
{
    public static class ZoneMapper
    {
        public static Zone ToDomain(AddZoneRequest request)
        {
            Zone zone = new Zone(
                name: request.Name,
                description: request.Description,
                deliveryDays: request.DeliveryDays.Select(d => (Day)Enum.Parse(typeof(Day), d)).ToList(),
                requestDays: request.RequestDays.Select(d => (Day)Enum.Parse(typeof(Day), d)).ToList()
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
                DeliveryDays = request.DeliveryDays.Select(d => (Day)Enum.Parse(typeof(Day), d)).ToList(),
                RequestDays = request.RequestDays.Select(d => (Day)Enum.Parse(typeof(Day), d)).ToList(),
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
                DeliveryDays = zone.DeliveryDays.Select(d => d.ToString()).ToList(),
                RequestDays = zone.RequestDays.Select(d => d.ToString()).ToList(),
                AuditInfo = AuditMapper.ToResponse(zone.AuditInfo)
            };
        }
    }
}
