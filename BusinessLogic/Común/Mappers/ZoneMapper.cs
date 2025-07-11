
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.Común.Mappers
{
    public static class ZoneMapper
    {
        public static Zone ToDomain(AddZoneRequest request)
        {
            return new Zone(
                id: 0,
                name: request.Name,
                description: request.Description,
                deliveryDays: request.DeliveryDays.Select(d => Enum.Parse<Day>(d, ignoreCase: true)).ToList(),
                requestDays: request.RequestDays.Select(d => Enum.Parse<Day>(d, ignoreCase: true)).ToList()
            );
        }
        public static Zone.UpdatableData ToUpdatableData(UpdateZoneRequest request)
        {
            return new Zone.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                DeliveryDays = request.DeliveryDays.Select(d => Enum.Parse<Day>(d, ignoreCase: true)).ToList(),
                RequestDays = request.RequestDays.Select(d => Enum.Parse<Day>(d, ignoreCase: true)).ToList()
            };
        }
        public static ZoneResponse ToResponse(Zone zone)
        {
            return new ZoneResponse
            {
                Id = zone.Id,
                Name = zone.Name,
                Description = zone.Description,
                DeliveryDays = zone.DeliveryDays.Select(d => d.ToString()).ToList(),
                RequestDays = zone.RequestDays.Select(d => d.ToString()).ToList()
            };
        }
    }
}