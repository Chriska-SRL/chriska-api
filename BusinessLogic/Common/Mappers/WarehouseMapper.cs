using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsWarehouse;

namespace BusinessLogic.Common.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse ToDomain(AddWarehouseRequest request)
        {
            var warehouse = new Warehouse(
                name: request.Name,
                description: request.Description
            );

            warehouse.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return warehouse;
        }

        public static Warehouse.UpdatableData ToUpdatableData(UpdateWarehouseRequest request)
        {
            return new Warehouse.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static WarehouseResponse ToResponse(Warehouse warehouse)
        {
            return new WarehouseResponse
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Description = warehouse.Description,
                Shelves = warehouse.Shelves.Select(ShelveMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(warehouse.AuditInfo)
            };
        }
    }
}
