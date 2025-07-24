using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsSubCategory
{
    public class UpdateSubCategoryRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
