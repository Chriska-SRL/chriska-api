using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsClient;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class ReceiptResponse : AuditableResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public ClientResponse? Client { get; set; }
    }
}
