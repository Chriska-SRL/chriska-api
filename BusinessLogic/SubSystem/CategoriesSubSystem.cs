using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddCategory(AddCategoryRequest category)
        {

            var newCategory = new Category(category.Name);

            newCategory.Validate();

            _categoryRepository.Add(newCategory);

        }
        public void UpdateCategory(UpdateCategoryRequest category)
        {

            var existingCategory = _categoryRepository.GetById(category.Id);
            if (existingCategory != null) throw new Exception("No se encontro la categoria");
            {
                existingCategory.Update(category.Name);
                _categoryRepository.Update(existingCategory);
            }
        }

        public void DeleteCategory(DeleteCategoryRequest deleteCategoryRequest)
        {
            var existingCategory = _categoryRepository.GetById(deleteCategoryRequest.Id);
            if (existingCategory != null) throw new Exception("No se encontro la categoria");
            {

                _categoryRepository.Delete(deleteCategoryRequest.Id);

            }
        }

        public List<CategoryResponse> GetAllCategory()
        {

            var list = _categoryRepository.GetAll();

            if (list == null) throw new Exception("No se encontraron categorias");

            var listCategoryResponse = new List<CategoryResponse>();


            foreach (var l in list)
            {
                var categoryResponse = new CategoryResponse
                {

                    Id = l.Id,
                    Name = l.Name

                };
                listCategoryResponse.Add(categoryResponse);
            }



            return listCategoryResponse;


        }

        public CategoryResponse GetCategoryById(int Id)
        {
            var existingCategory = _categoryRepository.GetById(Id);
            if (existingCategory != null) throw new Exception("No se encontro la categoria");
            {
                var ReturnCategory = new CategoryResponse
                {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name
                };
                return ReturnCategory;
            }

        }


        public void AddSubCategory(AddSubCategoryRequest subCategory)
        {

            var newSubCategory = new SubCategory(subCategory.Name, _categoryRepository.GetById(subCategory.CategoryId));

            newSubCategory.Validate();

            _subCategoryRepository.Add(newSubCategory);

        }

        public void UpdateSubCategory(UpdateSubCategoryRequest subCategory)
        {
            var existingSubCategory = _subCategoryRepository.GetById(subCategory.Id);
            if (existingSubCategory != null) throw new Exception("No se encontro la subcategoria");
            {
                existingSubCategory.Update(subCategory.Name);
                _subCategoryRepository.Update(existingSubCategory);
            }

        }
        public void DeleteSubCategory(DeleteSubCategoryRequest subCategoryRequest)
        {
            var existingSubCategory = _subCategoryRepository.GetById(subCategoryRequest.Id);
            if (existingSubCategory != null) throw new Exception("No se encontro la subcategoria");
            {
                _subCategoryRepository.Delete(subCategoryRequest.Id);
            }
        }

        public SubCategoryResponse GetSubCategoryById(int Id)
        {
            var existingSubCategory = _subCategoryRepository.GetById(Id);

            if (existingSubCategory != null) throw new Exception("No se encontro la subcategoria");
            {

                var subCategoryResponse = new SubCategoryResponse
                {
                    Id = existingSubCategory.Id,
                    Name = existingSubCategory.Name,
                    Category = new CategoryResponse
                    {
                        Id = existingSubCategory.Category.Id,
                        Name = existingSubCategory.Category.Name
                    }

                };

                return subCategoryResponse;
            }
        }

        public List<SubCategoryResponse> GetAllSubCategories()
        {
            var list = _subCategoryRepository.GetAll();
            if (list == null) throw new Exception("No se encontraron subcategorias");
            var listSubCategoryResponse = new List<SubCategoryResponse>();

            foreach (var l in list)
            {

                var categoryResponse = GetCategoryById(l.Category.Id);

                var subCategoryResponse = new SubCategoryResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    Category = new CategoryResponse
                    {

                        Id = l.Category.Id,
                        Name = l.Category.Name

                    }
                };

                listSubCategoryResponse.Add(subCategoryResponse);

            }

            return listSubCategoryResponse;

        }

    }
}
