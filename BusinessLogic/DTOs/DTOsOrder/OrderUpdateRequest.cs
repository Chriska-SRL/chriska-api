using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsOrder
{
    public class OrderUpdateRequest:AuditableRequest
    {
        public int Id { get; set; }
    }
}
