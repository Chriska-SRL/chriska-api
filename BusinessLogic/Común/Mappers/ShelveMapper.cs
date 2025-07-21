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
                stockMovements: new List<StockMovement>(),
                product: new List<Product>(),
                auditInfo: AuditMapper.ToDomain(addShelveRequest.AuditInfo)
                );
        }
        public static Shelve.UpdatableData ToUpdatableData(UpdateShelveRequest shelve)
        {
            return new Shelve.UpdatableData
            {
                Name = shelve.Name,
                Description = shelve.Description,
                Warehouse = new Warehouse(shelve.WarehouseId),
                AuditInfo = AuditMapper.ToDomain(shelve.AuditInfo)
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
                Stocks = shelve.StockMovements.Select(StockMovementMapper.ToResponse).ToList(),
                Products = shelve.Products.Select(ProductMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(shelve.AuditInfo)
            };
        }

        //public static ProductStockResponse ToResponse(ProductStock productStock)
        //{
        //    return new ProductStockResponse
        //    {
        //        Quantity = productStock.Quantity,
        //        Product = ProductMapper.ToResponse(productStock.Product)
        //    };
        //}

    }
}
