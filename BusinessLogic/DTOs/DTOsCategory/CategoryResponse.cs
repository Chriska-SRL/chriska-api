using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.DTOs.DTOsCategory
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SubCategoryResponse> SubCategories { get; set; } = new List<SubCategoryResponse>();


        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
