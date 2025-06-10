using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.Dominio;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly Facade _facade;

        public CategoriesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public ActionResult<List<CategoryResponse>> GetAllCategories()
        {
            try
            {
                return Ok(_facade.GetAllCategory());
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public ActionResult<CategoryResponse> GetCategoryById(int id)
        {
            try
            {
                return Ok(_facade.GetCategoryById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_CATEGORIES))]
        public ActionResult<CategoryResponse> AddCategory([FromBody] AddCategoryRequest request)
        {
            try
            {
                var response = _facade.AddCategory(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_CATEGORIES))]
        public ActionResult<CategoryResponse> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            try
            {
                var response = _facade.UpdateCategory(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(Permission.DELETE_CATEGORIES))]
        public ActionResult<CategoryResponse> DeleteCategory([FromBody] DeleteCategoryRequest request)
        {
            try
            {
                var response = _facade.DeleteCategory(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("subcategories")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public ActionResult<List<SubCategoryResponse>> GetAllSubCategories()
        {
            try
            {
                return Ok(_facade.GetAllSubCategories());
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("subcategories/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public ActionResult<SubCategoryResponse> GetSubCategoryById(int id)
        {
            try
            {
                return Ok(_facade.GetSubCategoryById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("subcategories")]
        [Authorize(Policy = nameof(Permission.CREATE_CATEGORIES))]
        public ActionResult<SubCategoryResponse> AddSubCategory([FromBody] AddSubCategoryRequest request)
        {
            try
            {
                var response = _facade.AddSubCategory(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("subcategories")]
        [Authorize(Policy = nameof(Permission.EDIT_CATEGORIES))]
        public ActionResult<SubCategoryResponse> UpdateSubCategory([FromBody] UpdateSubCategoryRequest request)
        {
            try
            {
                var response = _facade.UpdateSubCategory(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("subcategories")]
        [Authorize(Policy = nameof(Permission.DELETE_CATEGORIES))]
        public ActionResult<SubCategoryResponse> DeleteSubCategory([FromBody] DeleteSubCategoryRequest request)
        {
            try
            {
                var response = _facade.DeleteSubCategory(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
