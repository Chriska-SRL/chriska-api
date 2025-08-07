using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.DTOs.DTOsWarehouse;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class ShelveResponse : AuditableResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WarehouseResponse? Warehouse { get; set; }
    }
}
