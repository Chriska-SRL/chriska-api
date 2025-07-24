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
                name: dto.Name,
                auditInfo: AuditMapper.ToDomain(dto.AuditInfo)
            );
        }
        public static Category.UpdatableData ToUpdatableData(UpdateCategoryRequest dto)
        {
            return new Category.UpdatableData
            {
                Description = dto.Description,
                Name = dto.Name,
                AuditInfo = AuditMapper.ToDomain(dto.AuditInfo)
            };
        }
        public static CategoryResponse ToResponse(Category domain)
        {
            return new CategoryResponse{
                Id= domain.Id,
                Description = domain.Description,
                Name= domain.Name,
                SubCategories = domain.SubCategories.Select(SubCategoryMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(domain.AuditInfo)
            };
        }
    }
}
