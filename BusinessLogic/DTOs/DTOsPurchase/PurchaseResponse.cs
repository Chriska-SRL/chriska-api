using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProductItem;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class PurchaseResponse : AuditableResponse
    {
        public int Id { get; set; }
        public string? Observations { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public SupplierResponse? Supplier { get; set; }
        public List<ProductItemResponse> ProductItems { get; set; } = new();
    }
}