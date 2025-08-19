
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class DeliveryUpdateRequest:AuditableRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
    }
}
