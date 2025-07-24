using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsRole
{
    public class AddRoleRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Permissions { get; set; }
    }
}
