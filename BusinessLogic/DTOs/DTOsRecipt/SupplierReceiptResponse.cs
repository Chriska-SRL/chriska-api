using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class SupplierReceiptResponse : AuditableResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public SupplierResponse? Supplier { get; set; }
    }
}
