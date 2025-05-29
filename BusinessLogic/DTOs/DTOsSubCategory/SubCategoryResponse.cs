using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsSubCategory
{
    public class SubCategoryResponse
    {
        public string Name { get; set; }
        public CategoryResponse Category { get; set; }

    }
}
