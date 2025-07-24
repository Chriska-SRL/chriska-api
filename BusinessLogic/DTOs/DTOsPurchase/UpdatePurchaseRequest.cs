using BusinessLogic.DTOs.DTOsAudit;
namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class UpdatePurchaseRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int SupplierId { get; set; }
        public AuditInfoRequest AuditInfo { get; set; } = new AuditInfoRequest();
    }
}
