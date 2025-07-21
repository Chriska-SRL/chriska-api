using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs
{
    public class DeleteRequest:AuditInfoRequest
    {
        public int Id { get; set; }

    }
}
