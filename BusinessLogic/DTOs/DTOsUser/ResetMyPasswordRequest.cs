using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsUser
{
    public class ResetMyPasswordRequest:AuditableRequest
    {
        public string Username { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
