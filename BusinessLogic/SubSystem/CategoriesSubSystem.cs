using BusinessLogic.Dominio;
using BusinessLogic.DTOsCategory;
using BusinessLogic.DTOsSubCategory;
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

        public void AddCategory(AddSubCategoryRequest category)
        {

            var newCategory = new Category(category.Name);
            
            _categoryRepository.Add(newCategory);
        }




    }
}
