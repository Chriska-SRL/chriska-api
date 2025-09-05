using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class PurchaseAddRequest : AuditableRequest
    {
        public string? Observations { get; set; }
        public int SupplierId { get; set; }
        public string? InvoiceNumber { get; set; }
        public required DateTime Date { get; set; }
        public required List<ProductItemRequestForPurchase> ProductItems { get; set; }
    }
}