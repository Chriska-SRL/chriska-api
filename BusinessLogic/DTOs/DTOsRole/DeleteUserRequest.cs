
using BusinessLogic.Común;

namespace BusinessLogic.DTOs.DTOsRole
{
    public class DeleteUserRequest
    {
        public int Id { get; set; }
        public AuditInfo AuditInfo { get; set; }
    }
}
