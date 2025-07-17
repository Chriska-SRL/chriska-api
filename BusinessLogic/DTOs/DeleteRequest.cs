using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs
{
    public class DeleteRequest
    {
        public int Id { get; set; }
        public AuditInfoRequest AuditInfo { get; set; } = null!;
    }
}
