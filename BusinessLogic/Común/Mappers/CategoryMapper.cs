using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
namespace BusinessLogic.Común.Mappers
{
    public static class CategoryMapper
    {
        public static Category toDomain(AddCategoryRequest dto)
        {
            return new Category(
                id: 0,
                name: dto.Name
            );
        }
        public static Category.UpdatableData toDomain(UpdateCategoryRequest dto)
        {
            return new Category.UpdatableData
            {
                Name = dto.Name
            };
        }
        public static CategoryResponse toResponse(Category domain)
        {
            return new CategoryResponse{
                Id= domain.Id,
                Name= domain.Name
             };
        }
    }
}
