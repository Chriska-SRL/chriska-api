using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsCategory
{
    public class UpdateCategoryRequest: AuditableRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
