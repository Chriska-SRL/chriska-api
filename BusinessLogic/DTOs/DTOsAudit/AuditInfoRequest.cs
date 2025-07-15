using BusinessLogic.Común.Audits;

namespace BusinessLogic.DTOs.DTOsAudit
{
    public class AuditInfoRequest
    {
        public AuditLocation? Created { get; set; }
        public AuditLocation? Updated { get; set; }
        public AuditLocation? Deleted { get; set; }
    }
}