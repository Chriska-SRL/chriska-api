using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsBrand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Común.Mappers
{
    public static class BrandMapper
    {
        public static Brand ToDomain(AddBrandRequest brandRequest)
        {
            return new Brand
            (
                id: 0,
                name: brandRequest.Name,
                description: brandRequest.Description
            );
        }
        public static Brand.UpdatableData ToUpdatableData(UpdateBrandRequest brandRequest)
        {
            return new Brand.UpdatableData
            {
                Name = brandRequest.Name,
                Description = brandRequest.Description
            };
        }
        public static BrandResponse ToResponse(Brand brand)
        {
            return new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };
        }


    }
}
