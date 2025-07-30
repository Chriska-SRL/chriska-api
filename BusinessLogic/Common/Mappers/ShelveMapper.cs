using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsShelve;

namespace BusinessLogic.Común.Mappers
{
    public static class ShelveMapper
    {
        public static Shelve ToDomain(AddShelveRequest request)
        {
            var shelve = new Shelve(
                name: request.Name,
                description: request.Description,
                warehouse: new Warehouse(request.WarehouseId)
            );

            shelve.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return shelve;
        }

        public static Shelve.UpdatableData ToUpdatableData(UpdateShelveRequest request)
        {
            return new Shelve.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                Warehouse = new Warehouse(request.WarehouseId),
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static ShelveResponse ToResponse(Shelve shelve)
        {
            return new ShelveResponse
            {
                Id = shelve.Id,
                Name = shelve.Name,
                Description = shelve.Description,
                Warehouse = WarehouseMapper.ToResponse(shelve.Warehouse),
                AuditInfo = AuditMapper.ToResponse(shelve.AuditInfo)
            };
        }
    }
}
