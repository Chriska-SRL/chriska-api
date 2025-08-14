using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsProductItem
{
    public class ProductItemRequest : AuditableRequest
    { 
        public int Quantity { get; set; }
        public int? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0;
        public int ProductId { get; set; }
    }
}
