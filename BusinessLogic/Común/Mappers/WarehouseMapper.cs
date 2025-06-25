using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsWarehouse;

namespace BusinessLogic.Común.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse ToDomain(AddWarehouseRequest addWarehouseRequest)
        {
            return new Warehouse(
                id: 0,
                name: addWarehouseRequest.Name,
                description: addWarehouseRequest.Description,
                address: addWarehouseRequest.Address,
                shelves: new List<Shelve>()
            );
        }
        public static Warehouse.UpdatableData ToUpdatableData(UpdateWarehouseRequest updateWarehouseRequest)
        {
            return new Warehouse.UpdatableData
            {
                Name = updateWarehouseRequest.Name,
                Description = updateWarehouseRequest.Description,
                Address = updateWarehouseRequest.Address
            };
        }
        public static WarehouseResponse ToResponse(Warehouse warehouse)
        {
            return new WarehouseResponse
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Description = warehouse.Description,
                Address = warehouse.Address,
                Shelves = warehouse.Shelves.Select(ShelveMapper.ToResponse).ToList()
            };
        }
    }
}
    