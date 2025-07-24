using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;
using BusinessLogic.Común;

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

        public async Task<CategoryResponse> AddCategory(AddCategoryRequest request)
        {
            if (await _categoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var category = CategoryMapper.ToDomain(request);
            category.Validate();

            Category added = await _categoryRepository.AddAsync(category);
            return CategoryMapper.ToResponse(added);
        }

        public async Task<CategoryResponse> UpdateCategory(UpdateCategoryRequest request)
        {
            Category existing = await _categoryRepository.GetByIdAsync(request.Id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && _categoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var updatedData = CategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Category updated = await _categoryRepository.UpdateAsync(existing);
            return CategoryMapper.ToResponse(updated);
        }

        public async Task<CategoryResponse> DeleteCategory(DeleteCategoryRequest request)
        {
            Category deleted = await _categoryRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.Id));

            if (deleted.SubCategories.Any())
                throw new InvalidOperationException("No se puede eliminar una categoría que tiene subcategorías asociadas.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo); 
            deleted.SetDeletedAudit(auditInfo);                      

            await _categoryRepository.DeleteAsync(deleted);          

            return CategoryMapper.ToResponse(deleted);               
        }


        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(id));

            return CategoryMapper.ToResponse(category);
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync(QueryOptions options)
        {
            var categories = await _categoryRepository.GetAllAsync(options);
            return categories.Select(CategoryMapper.ToResponse).ToList();
        }

        // Subcategorías

        public async Task<SubCategoryResponse> AddSubCategory(AddSubCategoryRequest request)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId)
                                    ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.CategoryId));

            if (await _subCategoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            var subCategory = SubCategoryMapper.ToDomain(request);
            subCategory.Validate();

            SubCategory added = await _subCategoryRepository.AddAsync(subCategory);
            return SubCategoryMapper.ToResponse(added);
        }

        public async Task<SubCategoryResponse> UpdateSubCategory(UpdateSubCategoryRequest request)
        {
            var existing = await _subCategoryRepository.GetByIdAsync(request.Id)
                           ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && await _subCategoryRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            SubCategory.UpdatableData updatedData = SubCategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            SubCategory updated = await _subCategoryRepository.UpdateAsync(existing);
            return SubCategoryMapper.ToResponse(updated);
        }


        public async Task<SubCategoryResponse> DeleteSubCategory(DeleteSubCategoryRequest request)
        {
            var deleted = await _subCategoryRepository.GetByIdAsync(request.Id)
                           ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.Id));

            var associatedProducts = await _productRepository.GetBySubCategoryId(request.Id);

            if (associatedProducts.Any())
                throw new InvalidOperationException("No se puede eliminar una subcategoría que tiene productos asociados.");

            await _subCategoryRepository.DeleteAsync(deleted);

            return SubCategoryMapper.ToResponse(deleted);
        }

        public async Task<SubCategoryResponse> GetSubCategoryById(int id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id)
                              ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(id));

            return SubCategoryMapper.ToResponse(subCategory);
        }

        public async Task<List<SubCategoryResponse>> GetAllSubCategoriesAsync(QueryOptions options)
        {
            var subcategories = await _subCategoryRepository.GetAllAsync(options);
            return subcategories.Select(SubCategoryMapper.ToResponse).ToList();
        }
    }
}
