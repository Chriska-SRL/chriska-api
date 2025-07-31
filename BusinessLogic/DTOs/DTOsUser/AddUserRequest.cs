using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsUser
{
    public class AddUserRequest : AuditableRequest
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public string? Password { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
    }
}
