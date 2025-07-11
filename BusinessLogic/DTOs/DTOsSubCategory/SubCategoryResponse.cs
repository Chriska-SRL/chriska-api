﻿using BusinessLogic.DTOs.DTOsCategory;

namespace BusinessLogic.DTOs.DTOsSubCategory
{
    public class SubCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryResponse Category { get; set; }
    }
}
