
using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class ReturnRequestUpdateRequest:AuditableRequest
    {
        public int Id { get; set; }
    }
}
