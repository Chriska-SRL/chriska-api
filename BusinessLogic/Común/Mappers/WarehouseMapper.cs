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
        public static Warehouse toDomain(UpdateWarehouseRequest updateWarehouseRequest, int id)
        {
            return new Warehouse(
                id: id,
                name: updateWarehouseRequest.Name,
                description: updateWarehouseRequest.Description,
                address: updateWarehouseRequest.Address,
                shelves: new List<Shelve>()
            );
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
                    Id = s.Id,
                    Description = s.Description
                }).ToList()
            };
        }
    }
}
    