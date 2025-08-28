using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.Common.Mappers
{
    public static class CategoryMapper
    {
        public static Category ToDomain(AddCategoryRequest request)
        {
            Category category = new Category
            (
                request.Name,
                request.Description
            );
            category.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);

            return category;
        }

        public static Category.UpdatableData ToUpdatableData(UpdateCategoryRequest request)
        {
            return new Category.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static CategoryResponse? ToResponse(Category? category)
        {
            if (category == null) return null;
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories?.Select(SubCategoryMapper.ToResponse).OfType<SubCategoryResponse>().ToList() ?? null,
                AuditInfo = AuditMapper.ToResponse(category?.AuditInfo)
            };
        }
    }
}
