using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsCategory;

namespace BusinessLogic.Común.Mappers
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
            category.AuditInfo.SetCreated(request.getUserId(), request.Location);

            return category;
        }

        public static Category.UpdatableData ToUpdatableData(UpdateCategoryRequest request)
        {
            return new Category.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static CategoryResponse ToResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AuditInfo = AuditMapper.ToResponse(category.AuditInfo)
            };
        }
    }
}
