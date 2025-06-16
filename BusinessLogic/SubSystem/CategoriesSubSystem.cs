using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;

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

        public CategoryResponse AddCategory(AddCategoryRequest request)
        {
            if (_categoryRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var category = CategoryMapper.ToDomain(request);
            category.Validate();

            Category added = _categoryRepository.Add(category);
            return CategoryMapper.ToResponse(added);
        }

        public CategoryResponse UpdateCategory(UpdateCategoryRequest request)
        {
            var existing = _categoryRepository.GetById(request.Id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && _categoryRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var updatedData = CategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Category updated = _categoryRepository.Update(existing);
            return CategoryMapper.ToResponse(updated);
        }

        public CategoryResponse DeleteCategory(int id)
        {
            Category deleted = _categoryRepository.GetById(id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(id));

            if(deleted.SubCategories.Any())
                throw new InvalidOperationException("No se puede eliminar una categoría que tiene subcategorías asociadas.");

            _categoryRepository.Delete(id);

            return CategoryMapper.ToResponse(deleted);
        }

        public CategoryResponse GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(id));

            return CategoryMapper.ToResponse(category);
        }

        public List<CategoryResponse> GetAllCategory()
        {
            return _categoryRepository.GetAll()
                                      .Select(CategoryMapper.ToResponse)
                                      .ToList();
        }

        // Subcategorías

        public SubCategoryResponse AddSubCategory(AddSubCategoryRequest request)
        {
            var existing = _categoryRepository.GetById(request.CategoryId)
                       ?? throw new ArgumentException("Categoría no encontrada.", nameof(request.CategoryId));

            if (_subCategoryRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            var subCategory = SubCategoryMapper.ToDomain(request);
            subCategory.Validate();

            SubCategory added = _subCategoryRepository.Add(subCategory);
            return SubCategoryMapper.ToResponse(added);
        }

        public SubCategoryResponse UpdateSubCategory(UpdateSubCategoryRequest request)
        {
            var existing = _subCategoryRepository.GetById(request.Id)
                           ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && _subCategoryRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            SubCategory.UpdatableData updatedData = SubCategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            SubCategory updated = _subCategoryRepository.Update(existing);
            return SubCategoryMapper.ToResponse(updated);
        }

        public SubCategoryResponse DeleteSubCategory(int id)
        {
            var deleted = _subCategoryRepository.GetById(id)
                           ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(id));

            if (_productRepository.GetBySubCategoryId(id).Count() > 0)
                throw new InvalidOperationException("No se puede eliminar una subcategoría que tiene productos asociados.");

            _subCategoryRepository.Delete(id);

            return SubCategoryMapper.ToResponse(deleted);
        }

        public SubCategoryResponse GetSubCategoryById(int id)
        {
            var subCategory = _subCategoryRepository.GetById(id)
                              ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(id));

            return SubCategoryMapper.ToResponse(subCategory);
        }

        public List<SubCategoryResponse> GetAllSubCategories()
        {
            return _subCategoryRepository.GetAll()
                                         .Select(SubCategoryMapper.ToResponse)
                                         .ToList();
        }
    }
}
