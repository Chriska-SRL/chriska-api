using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs
{
    public class DeleteRequest:AuditableRequest
    {
        public int Id { get; set; }

    }
}
