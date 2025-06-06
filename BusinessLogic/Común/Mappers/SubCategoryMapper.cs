using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static SubCategoryResponse ToResponse(SubCategory subCategory)
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
        public static SubCategory.UpdatableData ToUpdatableData(UpdateSubCategoryRequest dto)
        {
            return new SubCategory.UpdatableData
            {
                Name = dto.Name
            };
        }
    }
}
