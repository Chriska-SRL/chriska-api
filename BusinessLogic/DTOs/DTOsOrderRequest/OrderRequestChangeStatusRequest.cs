using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsOrderRequest
{
    public class OrderRequestChangeStatusRequest:AuditableRequest
    {
        public Status Status { get; set; }
    }
}
