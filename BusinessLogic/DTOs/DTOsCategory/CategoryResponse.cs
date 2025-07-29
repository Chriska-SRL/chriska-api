using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.DTOs.DTOsCategory
{
    public class CategoryResponse:AuditableResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SubCategoryResponse> SubCategories { get; set; } = new List<SubCategoryResponse>();
    }
}
