using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class ReceiptAddRequest : AuditableRequest
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public int ClientId { get; set; }
    }
}
