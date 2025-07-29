using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs;
using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class BrandSubSystem
    {
        private readonly IBrandRepository _brandRepository;

        public BrandSubSystem(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<BrandResponse> AddBrandAsync(AddBrandRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BrandResponse> UpdateBrandAsync(UpdateBrandRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BrandResponse> DeleteBrandAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BrandResponse> GetBrandByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BrandResponse>> GetAllBrandsAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
