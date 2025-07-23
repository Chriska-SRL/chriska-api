using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsBrand
{
    public class UpdateBrandRequest: AuditableRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }

    }
}
