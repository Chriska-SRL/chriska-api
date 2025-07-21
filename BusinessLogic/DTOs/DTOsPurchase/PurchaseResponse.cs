using BusinessLogic.Común;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsSupplier;


namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class PurchaseResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public SupplierResponse Supplier { get; set; }
        public AuditInfoResponse AuditInfo { get; set; } = new AuditInfoResponse();

    }
}
