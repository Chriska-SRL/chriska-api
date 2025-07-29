using BusinessLogic.Común.Audits;

namespace BusinessLogic.DTOs.DTOsAudit
{
    public class AuditInfoResponse
    {
        public AuditAction? Created { get; set; }
        public AuditAction? Updated { get; set; }
        public AuditAction? Deleted { get; set; }
    }
}

