
namespace BusinessLogic.DTOs.DTOsAudit
{
    public abstract class AuditableResponse
    {
        public AuditInfoResponse AuditInfo { get; set; } = new AuditInfoResponse();
    }
}
