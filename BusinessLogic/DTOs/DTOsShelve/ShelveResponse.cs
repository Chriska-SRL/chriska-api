using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.DTOs.DTOsWarehouse;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class ShelveResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WarehouseResponse Warehouse { get; set; }
        public List<ProductStockResponse> Stocks { get; set; }
        public List<StockMovementResponse> StockMovements { get; set; }
    }
}
