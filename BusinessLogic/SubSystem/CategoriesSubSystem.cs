using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class CategoriesSubSystem
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public CategoriesSubSystem(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        // Categorías

        public CategoryResponse AddCategory(AddCategoryRequest request)
        {
            if (_categoryRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una categoría con el mismo nombre.", nameof(request.Name));

            var category = CategoryMapper.ToDomain(request);
            category.Validate();

            var added = _categoryRepository.Add(category);
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

            var updated = _categoryRepository.Update(existing);
            return CategoryMapper.ToResponse(updated);
        }

        public CategoryResponse DeleteCategory(int id)
        {
            var deleted = _categoryRepository.Delete(id)
                          ?? throw new ArgumentException("Categoría no encontrada.", nameof(id));

            //TODO: Implementar control de integridad referencial: no se puede borrar si hay Subcategorias asociadas

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

            var added = _subCategoryRepository.Add(subCategory);
            return SubCategoryMapper.ToResponse(added);
        }

        public SubCategoryResponse UpdateSubCategory(UpdateSubCategoryRequest request)
        {
            var existing = _subCategoryRepository.GetById(request.Id)
                           ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && _subCategoryRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una subcategoría con el mismo nombre.", nameof(request.Name));

            var updatedData = SubCategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = _subCategoryRepository.Update(existing);
            return SubCategoryMapper.ToResponse(updated);
        }

        public SubCategoryResponse DeleteSubCategory(int id)
        {
            var deleted = _subCategoryRepository.Delete(id)
                           ?? throw new ArgumentException("Subcategoría no encontrada.", nameof(id));

            //TODO: Implementar control de integridad referencial: no se puede borrar si hay productos asociados

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
