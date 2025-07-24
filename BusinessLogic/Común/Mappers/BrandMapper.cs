using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.Mappers;
using BusinessLogic.Común.Audits;

namespace BusinessLogic.Común.Mappers
{
    public static class BrandMapper
    {
        public static Brand ToDomain(AddBrandRequest request)
        {
            return new Brand(
                id: 0,
                name: request.Name,
                description: request.Description,
                auditInfo: AuditMapper.ToDomain(request.AuditInfo)
            );
        }

        public static Brand.UpdatableData ToUpdatableData(UpdateBrandRequest request)
        {
            return new Brand.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                AuditInfo = AuditMapper.ToDomain(request.AuditInfo)
            };
        }

        public static BrandResponse ToResponse(Brand brand)
        {
            return new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                AuditInfo = AuditMapper.ToResponse(brand.AuditInfo)
            };
        }
    }
}
