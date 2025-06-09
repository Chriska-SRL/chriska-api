using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.Común.Mappers
{
    public static class ZoneMapper
    {
        public static Zone toDomain(AddZoneRequest request)
        {
            return new Zone(
                id:0,
                name:request.Name,
                description: request.Description,
                days: new List<Day>()
            );
        }
        public static Zone.UpdatableData toDomain(UpdateZoneRequest request)
        {
            return new Zone.UpdatableData
            {
                Name = request.Name,
                Description = request.Description
            };
        }
        public static ZoneResponse toResponse(Zone zone)
        {
            return new ZoneResponse
            {
                Id = zone.Id,
                Name = zone.Name,
                Description = zone.Description   
                //Lista de dias que se hacen entregas?
            };
        }
    }
}
