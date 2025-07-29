using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsSubCategory
{
    public class AddSubCategoryRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
