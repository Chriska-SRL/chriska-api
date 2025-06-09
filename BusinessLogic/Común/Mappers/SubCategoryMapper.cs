using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.Común.Mappers
{
    public static class SubCategoryMapper
    {
        public static SubCategory toDomain(AddSubCategoryRequest dto) {
            return new SubCategory(
                id: 0, 
                name: dto.Name,
                category: new Category (id:dto.CategoryId,name:dto.Name)
            );
        }
        public static SubCategoryResponse toResponse(SubCategory subCategory)
        {
            return new SubCategoryResponse
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                Category = new CategoryResponse
                {
                    Id = subCategory.Category.Id,
                    Name = subCategory.Category.Name
                }
            };
        }
        public static SubCategory.UpdatableData toDomain(UpdateSubCategoryRequest dto)
        {
            return new SubCategory.UpdatableData
            {
                Name = dto.Name
            };
        }
    }
}
