using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsShelve;

namespace BusinessLogic.Común.Mappers
{
    public static class ShelveMapper
    {
        public static Shelve ToDomain(AddShelveRequest addShelveRequest)
        {
            return new Shelve(
                id: 0,
                name: addShelveRequest.Name,
                description: addShelveRequest.Description,
                warehouse: new Warehouse(addShelveRequest.WarehouseId),
                productStocks: new List<ProductStock>(),
                stockMovements: new List<StockMovement>()
                );
        }
        public static Shelve.UpdatableData ToUpdatableData(UpdateShelveRequest shelve)
        {
            return new Shelve.UpdatableData
            {
                Name = shelve.Name,
                Description = shelve.Description,
                Warehouse = new Warehouse(shelve.WarehouseId)
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
                Stocks = shelve.Stocks.Select(ToResponse).ToList()
            };
        }

        public static ProductStockResponse ToResponse(ProductStock productStock)
        {
            return new ProductStockResponse
            {
                Quantity = productStock.Quantity,
                Product = ProductMapper.ToResponse(productStock.Product)
            };
        }
    }
}
