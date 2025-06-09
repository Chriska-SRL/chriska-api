using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.Común.Mappers
{
    public static class SubCategoryMapper
    {
        public static SubCategory ToDomain(AddSubCategoryRequest dto)
        {
            return new SubCategory(
                id: 0,
                name: dto.Name,
                description: dto.Description,
                category: new Category(dto.CategoryId, string.Empty, string.Empty)
            );
        }

        public static SubCategory.UpdatableData ToUpdatableData(UpdateSubCategoryRequest dto)
        {
            return new SubCategory.UpdatableData
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }

        public static SubCategoryResponse ToResponse(SubCategory domain)
        {
            return new SubCategoryResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                Description = domain.Description,
                Category = new DTOs.DTOsCategory.CategoryResponse
                {
                    Id = domain.Category.Id,
                    Name = domain.Category.Name,
                    Description = domain.Category.Description
                }
            };
        }
    }
}
