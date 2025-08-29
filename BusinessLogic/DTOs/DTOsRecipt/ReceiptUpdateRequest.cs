using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class ReceiptUpdateRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string? Notes { get; set; }
    }
}
