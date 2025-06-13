using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsWarehouse;

namespace BusinessLogic.Común.Mappers
{
    public static class ShelveMapper
    {
        public static Shelve ToDomain(AddShelveRequest addShelveRequest)
        {
            return new Shelve(
                id: 0,
                description: addShelveRequest.Description,
                warehouse: new Warehouse(addShelveRequest.WarehouseId, string.Empty, string.Empty, string.Empty, new List<Shelve>()),
                productStocks: new List<ProductStock>(),
                stockMovements: new List<StockMovement>()
                );
        }
        public static Shelve.UpdatableData ToUpdatableData(UpdateShelveRequest shelve)
        {
            return new Shelve.UpdatableData
            {
                Description = shelve.Description,
                Warehouse = new Warehouse(shelve.WarehouseId, string.Empty, string.Empty, string.Empty, new List<Shelve>())
            };
        }
        public static ShelveResponse ToResponse(Shelve shelve)
        {
            return new ShelveResponse
            {
                Description = shelve.Description,
                Warehouse = new WarehouseResponse
                {
                    Id = shelve.Warehouse.Id,
                    Name = shelve.Warehouse.Name,
                    Description = shelve.Warehouse.Description,
                    Address = shelve.Warehouse.Address
                },
            };
        }
    }
}
