
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class DeliveryAddRequest:AuditableRequest
    {
        public int Id { get; set; }
        public string Observations { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int Crates { get; set; }
        public int OrderId { get; set; }

    }
}
