using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsShelve;

namespace BusinessLogic.Common.Mappers
{
    public static class ShelveMapper
    {
        public static Shelve ToDomain(AddShelveRequest request, Warehouse warehouse)
        {
            var shelve = new Shelve(
                name: request.Name,
                description: request.Description,
                warehouse: warehouse
            );

            shelve.AuditInfo?.SetCreated(request.getUserId(), request.Location);
            return shelve;
        }

        public static Shelve.UpdatableData ToUpdatableData(UpdateShelveRequest request, Warehouse warehouse)
        {
            return new Shelve.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                Warehouse = warehouse,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static ShelveResponse? ToResponse(Shelve? shelve)
        {
            if (shelve == null) return null;
            return new ShelveResponse
            {
                Id = shelve.Id,
                Name = shelve.Name,
                Description = shelve.Description,
                Warehouse = shelve.Warehouse != null ? WarehouseMapper.ToResponse(shelve.Warehouse) : null,
                AuditInfo = AuditMapper.ToResponse(shelve.AuditInfo)
            };
        }
    }
}
