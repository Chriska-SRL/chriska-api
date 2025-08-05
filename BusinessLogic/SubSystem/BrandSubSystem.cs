using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs;
using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;

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
            var newBrand = BrandMapper.ToDomain(request);
            newBrand.Validate();

            var existing = await _brandRepository.GetByNameAsync(newBrand.Name);
            if (existing != null)
                throw new ArgumentException("Ya existe una marca con ese nombre.");

            var added = await _brandRepository.AddAsync(newBrand);
            return BrandMapper.ToResponse(added);
        }

        public async Task<BrandResponse> UpdateBrandAsync(UpdateBrandRequest request)
        {
            var existingBrand = await _brandRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la marca seleccionada.");

            var existing = await _brandRepository.GetByNameAsync(request.Name);
            if (existingBrand.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe una marca con ese nombre.");

            var updatedData = BrandMapper.ToUpdatableData(request);
            existingBrand.Update(updatedData);

            var updated = await _brandRepository.UpdateAsync(existingBrand);
            return BrandMapper.ToResponse(updated);
        }

        public async Task<BrandResponse> DeleteBrandAsync(DeleteRequest request)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la marca seleccionada.");

            brand.MarkAsDeleted(request.getUserId(), request.Location);
            await _brandRepository.DeleteAsync(brand);
            return BrandMapper.ToResponse(brand);
        }

        public async Task<BrandResponse> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la marca seleccionada.");

            return BrandMapper.ToResponse(brand);
        }

        public async Task<List<BrandResponse>> GetAllBrandsAsync(QueryOptions options)
        {
            var brands = await _brandRepository.GetAllAsync(options);
            return brands.Select(BrandMapper.ToResponse).ToList();
        }
    }
}
