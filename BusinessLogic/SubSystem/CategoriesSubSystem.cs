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

        public CategoriesSubSystem(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        // Categorías

        public CategoryResponse AddCategory(AddCategoryRequest request)
        {
            Category category = CategoryMapper.ToDomain(request);
            category.Validate();

            Category added = _categoryRepository.Add(category);
            return CategoryMapper.ToResponse(added);
        }

        public CategoryResponse UpdateCategory(UpdateCategoryRequest request)
        {
            Category existing = _categoryRepository.GetById(request.Id)
                          ?? throw new InvalidOperationException("Categoría no encontrada.");

            Category.UpdatableData updatedData = CategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Category updated = _categoryRepository.Update(existing);
            return CategoryMapper.ToResponse(updated);
        }

        public CategoryResponse DeleteCategory(DeleteCategoryRequest request)
        {
            Category deleted = _categoryRepository.Delete(request.Id)
                          ?? throw new InvalidOperationException("Categoría no encontrada.");

            return CategoryMapper.ToResponse(deleted);
        }

        public CategoryResponse GetCategoryById(int id)
        {
            Category category = _categoryRepository.GetById(id)
                          ?? throw new InvalidOperationException("Categoría no encontrada.");

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
            SubCategory subCategory = SubCategoryMapper.ToDomain(request);
            subCategory.Validate();

            SubCategory added = _subCategoryRepository.Add(subCategory);
            return SubCategoryMapper.ToResponse(added);
        }

        public SubCategoryResponse UpdateSubCategory(UpdateSubCategoryRequest request)
        {
            SubCategory existing = _subCategoryRepository.GetById(request.Id)
                           ?? throw new InvalidOperationException("Subcategoría no encontrada.");

            SubCategory.UpdatableData updatedData = SubCategoryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            SubCategory updated = _subCategoryRepository.Update(existing);
            return SubCategoryMapper.ToResponse(updated);
        }

        public SubCategoryResponse DeleteSubCategory(DeleteSubCategoryRequest request)
        {
            SubCategory deleted = _subCategoryRepository.Delete(request.Id)
                           ?? throw new InvalidOperationException("Subcategoría no encontrada.");

            return SubCategoryMapper.ToResponse(deleted);
        }

        public SubCategoryResponse GetSubCategoryById(int id)
        {
            SubCategory subCategory = _subCategoryRepository.GetById(id)
                              ?? throw new InvalidOperationException("Subcategoría no encontrada.");

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
