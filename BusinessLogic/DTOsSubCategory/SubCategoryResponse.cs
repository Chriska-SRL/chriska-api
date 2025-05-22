using BusinessLogic.Dominio;
using BusinessLogic.DTOsCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsSubCategory
{
    public class SubCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryResponse Category { get; set; }

    }
}
