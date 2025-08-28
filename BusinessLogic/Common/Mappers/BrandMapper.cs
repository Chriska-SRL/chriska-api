using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsBrand;

namespace BusinessLogic.Common.Mappers
{
    public static class BrandMapper
    {
        public static Brand ToDomain(AddBrandRequest request)
        {
            Brand brand = new Brand
            (
                request.Name,
                request.Description
            );
            brand.AuditInfo?.SetCreated(request.getUserId(),request.AuditLocation);

            return brand;
        }

        public static Brand.UpdatableData ToUpdatableData(UpdateBrandRequest request)
        {
            return new Brand.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static BrandResponse? ToResponse(Brand? brand)
        {
            if(brand == null) return null;
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
