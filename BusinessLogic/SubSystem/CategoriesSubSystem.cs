using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class CategoriesSubSystem
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoriesSubSystem(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productRepository = productRepository;
        }

        // Categorías

        public async Task<CategoryResponse> AddCategoryAsync(AddCategoryRequest request)
        {
            if (await _categoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var category = CategoryMapper.ToDomain(request);
            category.Validate();

            var added = await _categoryRepository.AddAsync(category);
            return CategoryMapper.ToResponse(added);
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            var existing = await _categoryRepository.GetByIdAsync(request.Id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && await _categoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var updatedData = CategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _categoryRepository.UpdateAsync(existing);
            return CategoryMapper.ToResponse(updated);
        }

        public async Task<CategoryResponse> DeleteCategoryAsync(DeleteRequest request)
        {
            var deleted = await _categoryRepository.GetByIdAsync(request.Id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.Id));

            deleted.MarkAsDeleted(request.getUserId(), request.Location);
            await _categoryRepository.DeleteAsync(deleted);
            return CategoryMapper.ToResponse(deleted);
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(id));

            return CategoryMapper.ToResponse(category);
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync(QueryOptions options)
        {
            var category = await _categoryRepository.GetAllAsync(options);
            return category.Select(CategoryMapper.ToResponse).ToList();
        }

        //SubCategorias

        public async Task<SubCategoryResponse> AddSubCategoryAsync(AddSubCategoryRequest request)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId)
                ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.CategoryId));

            if (await _subCategoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            var subCategory = SubCategoryMapper.ToDomain(request);
            subCategory.Validate();

            var added = await _subCategoryRepository.AddAsync(subCategory);
            return SubCategoryMapper.ToResponse(added);
        }

        public async Task<SubCategoryResponse> UpdateSubCategoryAsync(UpdateSubCategoryRequest request)
        {
            var existing = await _subCategoryRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && await _subCategoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            var updatedData = SubCategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _subCategoryRepository.UpdateAsync(existing);
            return SubCategoryMapper.ToResponse(updated);
        }

        public async Task<SubCategoryResponse> DeleteSubCategoryAsync(DeleteRequest request)
        {
            var deleted = await _subCategoryRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.Id));

            //var associatedProducts = await _productRepository.GetBySubCategoryIdAsync(request.Id);
            //if (associatedProducts.Any())
                //throw new InvalidOperationException("No se puede eliminar una subcategoría que tiene productos asociados.");

            deleted.MarkAsDeleted(request.getUserId(), request.Location);
            await _subCategoryRepository.DeleteAsync(deleted);

            return SubCategoryMapper.ToResponse(deleted);
        }

        public async Task<SubCategoryResponse> GetSubCategoryByIdAsync(int id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(id));

            return SubCategoryMapper.ToResponse(subCategory);
        }

        public async Task<List<SubCategoryResponse>> GetAllSubCategoriesAsync(QueryOptions options)
        {
            var subCategories = await _subCategoryRepository.GetAllAsync(options);
            return subCategories.Select(SubCategoryMapper.ToResponse).ToList();
        }

    }
}
