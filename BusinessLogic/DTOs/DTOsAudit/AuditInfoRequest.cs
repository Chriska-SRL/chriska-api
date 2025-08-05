using BusinessLogic.Common.Audits;

namespace BusinessLogic.DTOs.DTOsAudit
{
    public class AuditInfoRequest
    {
        public Audit Created { get; set; } = new Audit();
        public Audit Updated { get; set; } = new Audit();
        public Audit Deleted { get; set; } = new Audit();
    }
}