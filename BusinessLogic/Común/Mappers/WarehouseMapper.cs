using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsWarehouse;

namespace BusinessLogic.Común.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse toDomain(AddWarehouseRequest addWarehouseRequest)
        {
            return new Warehouse(
                id: 0,
                name: addWarehouseRequest.Name,
                description: addWarehouseRequest.Description,
                address: addWarehouseRequest.Address,
                shelves: new List<Shelve>()
            );
        }
        public static Warehouse.UpdatableData toDomain(UpdateWarehouseRequest updateWarehouseRequest)
        {
            return new Warehouse.UpdatableData
            {
                Name = updateWarehouseRequest.Name,
                Description = updateWarehouseRequest.Description,
                Address = updateWarehouseRequest.Address
            };
        }
        public static WarehouseResponse toResponse(Warehouse warehouse)
        {
            return new WarehouseResponse
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Description = warehouse.Description,
                Address = warehouse.Address,
                Shelves = warehouse.Shelves.Select(s => new ShelveResponse
                {
                    Description = s.Description
                }).ToList()
            };
        }
    }
}
    