using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class ReturnRequestAddRequest:AuditableRequest
    {
        public int DeliveryId { get; set; }
    }

}
