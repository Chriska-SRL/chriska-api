using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class PurchaseUpdateRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string? Observations { get; set; }
        public int SupplierId { get; set; }
        public string? InvoiceNumber { get; set; }
        public List<ProductItemRequestForPurchase> ProductItems { get; set; } = new();
    }
}