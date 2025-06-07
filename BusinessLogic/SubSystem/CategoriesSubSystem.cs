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

        public void AddCategory(AddCategoryRequest addCategory)
        {
            Category category = CategoryMapper.toDomain(addCategory);
            _categoryRepository.Add(category);
        }

        public void UpdateCategory(UpdateCategoryRequest updateCategory)
        {
            Category existingCategory = _categoryRepository.GetById(updateCategory.Id);
            if (existingCategory == null) throw new Exception("No se encontró la categoría");
            existingCategory.Update(CategoryMapper.toDomain(updateCategory));
            _categoryRepository.Update(existingCategory);
        }

        public void DeleteCategory(DeleteCategoryRequest deleteCategoryRequest)
        {
            Category existingCategory = _categoryRepository.GetById(deleteCategoryRequest.Id);
            if (existingCategory == null) throw new Exception("No se encontro la categoria");           
                _categoryRepository.Delete(deleteCategoryRequest.Id);           
        }

        public List<CategoryResponse> GetAllCategory()
        {
            List<Category> listCategory = _categoryRepository.GetAll();
            if (!listCategory.Any()) throw new Exception("No se encontraron categorias");
            return listCategory.Select(CategoryMapper.toResponse).ToList();
        }

        public CategoryResponse GetCategoryById(int Id)
        {
            Category existingCategory = _categoryRepository.GetById(Id);
            if (existingCategory == null) throw new Exception("No se encontro la categoria");          
            CategoryResponse ReturnCategory = CategoryMapper.toResponse(existingCategory);
            return ReturnCategory;          
        }

        public void AddSubCategory(AddSubCategoryRequest addSubCategory)
        {
            SubCategory subCategory = SubCategoryMapper.toDomain(addSubCategory);
            subCategory.Validate();
            _subCategoryRepository.Add(subCategory);
        }

        public void UpdateSubCategory(UpdateSubCategoryRequest updateSubCategory)
        {
            SubCategory existingCategory = _subCategoryRepository.GetById(updateSubCategory.Id);
            if (existingCategory == null) throw new Exception("No se encontró la categoría");
            existingCategory.Update(SubCategoryMapper.toDomain(updateSubCategory));
            _subCategoryRepository.Update(existingCategory);
        }

        public void DeleteSubCategory(DeleteSubCategoryRequest subCategoryRequest)
        {
            SubCategory existingSubCategory = _subCategoryRepository.GetById(subCategoryRequest.Id);
            if (existingSubCategory == null) throw new Exception("No se encontro la subcategoria");
                _subCategoryRepository.Delete(subCategoryRequest.Id);     
        }

        public SubCategoryResponse GetSubCategoryById(int Id)
        {
            SubCategory existingSubCategory = _subCategoryRepository.GetById(Id);
            if (existingSubCategory == null) throw new Exception("No se encontro la subcategoria");    
            return SubCategoryMapper.toResponse(existingSubCategory);          
        }

        public List<SubCategoryResponse> GetAllSubCategories()
        {
            List<SubCategory> listSubCategory = _subCategoryRepository.GetAll();
            if (!listSubCategory.Any()) throw new Exception("No se encontraron subcategorias");
            return listSubCategory.Select(SubCategoryMapper.toResponse).ToList();
        }
    }
}
