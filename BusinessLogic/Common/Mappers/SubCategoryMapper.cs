using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.Common.Mappers
{
    public static class SubCategoryMapper
    {
        public static SubCategory ToDomain(AddSubCategoryRequest request, Category category)
        {
            var subCategory = new SubCategory(
                name: request.Name,
                description: request.Description,
                category: category
            );

            subCategory.AuditInfo?.SetCreated(request.getUserId(), request.Location);
            return subCategory;
        }

        public static SubCategory.UpdatableData ToUpdatableData(UpdateSubCategoryRequest request)
        {
            return new SubCategory.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static SubCategoryResponse? ToResponse(SubCategory? subCategory)
        {
            if(subCategory == null) return null;
            return new SubCategoryResponse
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                Description = subCategory.Description,
                Category = CategoryMapper.ToResponse(subCategory.Category),
                AuditInfo = AuditMapper.ToResponse(subCategory.AuditInfo)
            };
        }
    }
}
