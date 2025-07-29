using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsUser
{
    public class ResetPasswordRequest : AuditableRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
