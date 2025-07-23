using BusinessLogic.Común;
namespace BusinessLogic.DTOs.DTOsAudit
{
    public abstract class AuditableRequest
    {
        public AuditInfoRequest AuditInfo { get; set; } = new AuditInfoRequest();
    }
}
