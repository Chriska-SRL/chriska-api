using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class SupplierReceiptAddRequest : AuditableRequest
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int SupplierId { get; set; }
    }
}
