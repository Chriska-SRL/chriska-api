using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
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
            throw new NotImplementedException();
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCategoryAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        // Subcategorías

        public async Task<SubCategoryResponse> AddSubCategoryAsync(AddSubCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SubCategoryResponse> UpdateSubCategoryAsync(UpdateSubCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteSubCategoryAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SubCategoryResponse> GetSubCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubCategoryResponse>> GetAllSubCategoriesAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
