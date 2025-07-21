using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsCategory
{
    public class AddCategoryRequest:AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
