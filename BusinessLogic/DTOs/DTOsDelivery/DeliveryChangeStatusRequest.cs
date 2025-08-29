using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class DeliveryChangeStatusRequest:AuditableRequest
    {
        public Status Status { get; set; }
        public decimal? Amount { get; set; } = null;
        public PaymentMethod? PaymentMethod { get; set; } = null;
        public int Crates { get; set; } = 0;
    }
}
