using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.DTOs.DTOsShelve;

namespace BusinessLogic.Common.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse ToDomain(AddWarehouseRequest request)
        {
            Warehouse warehouse = new Warehouse
            (
                request.Name,
                request.Description
            );
            warehouse.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);

            return warehouse;
        }

        public static Warehouse.UpdatableData ToUpdatableData(UpdateWarehouseRequest request)
        {
            return new Warehouse.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static WarehouseResponse? ToResponse(Warehouse? warehouse)
        {
            if (warehouse == null) return null;
            return new WarehouseResponse
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Description = warehouse.Description,
                Shelves = warehouse.Shelves?.Select(ShelveMapper.ToResponse).OfType<ShelveResponse>().ToList() ?? null,
                AuditInfo = AuditMapper.ToResponse(warehouse.AuditInfo)
            };
        }
    }
}
