using BusinessLogic.Dominio;
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

        private List<Category> Categories = new List<Category>();

        private List<SubCategory> SubCategories = new List<SubCategory>();

        private readonly ICategoryRepository _categoryRepository;

        public CategoriesSubSystem(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }




    }
}
