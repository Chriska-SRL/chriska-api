
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class AddReceiptRequest
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public AuditInfoRequest AuditInfo { get; set; } = new AuditInfoRequest();
    }
}
