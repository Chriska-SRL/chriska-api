using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static Category toDomain(UpdateCategoryRequest dto)
        {
            return new Category(
                id: dto.Id,
                name: dto.Name
            );
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
