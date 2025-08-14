using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDocumentClient
{
    public class DocumentClientChangeStatusRequest:AuditableRequest
    {
        public Status Status { get; set; }
    }
}
