using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsStockMovement
{
    public class AddStockMovementRequest : AuditableRequest
    {
        public DateTime? Date { get; set; }
        public decimal Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; }
        public int ProductId { get; set; }
    }
}
