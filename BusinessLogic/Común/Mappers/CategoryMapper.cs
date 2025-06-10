using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;

namespace BusinessLogic.Común.Mappers
{
    public static class CategoryMapper
    {
        public static Category ToDomain(AddCategoryRequest dto)
        {
            return new Category(
                id: 0,
                description: dto.Description,
                name: dto.Name
            );
        }
        public static Category.UpdatableData ToUpdatableData(UpdateCategoryRequest dto)
        {
            return new Category.UpdatableData
            {
                Description = dto.Description,
                Name = dto.Name
            };
        }
        public static CategoryResponse ToResponse(Category domain)
        {
            return new CategoryResponse{
                Id= domain.Id,
                Description = domain.Description,
                Name= domain.Name
             };
        }
    }
}
