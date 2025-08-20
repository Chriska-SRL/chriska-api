
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class DeliveryUpdateRequest:AuditableRequest
    {
        public int Id { get; set; }
        public string Observations { get; set; } = string.Empty;
    }
}
