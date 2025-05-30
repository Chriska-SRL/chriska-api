using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly Facade _facade;

        public CategoriesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpGet]
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
        public IActionResult AddCategory([FromBody] AddCategoryRequest request)
        {
            try
            {
                _facade.AddCategory(request);
                return Ok(new { message = "Categoría agregada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            try
            {
                _facade.UpdateCategory(request);
                return Ok(new { message = "Categoría actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteCategory([FromBody] DeleteCategoryRequest request)
        {
            try
            {
                _facade.DeleteCategory(request);
                return Ok(new { message = "Categoría eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("subcategories")]
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
        public IActionResult AddSubCategory([FromBody] AddSubCategoryRequest request)
        {
            try
            {
                _facade.AddSubCategory(request);
                return Ok(new { message = "Subcategoría agregada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("subcategories")]
        public IActionResult UpdateSubCategory([FromBody] UpdateSubCategoryRequest request)
        {
            try
            {
                _facade.UpdateSubCategory(request);
                return Ok(new { message = "Subcategoría actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("subcategories")]
        public IActionResult DeleteSubCategory([FromBody] DeleteSubCategoryRequest request)
        {
            try
            {
                _facade.DeleteSubCategory(request);
                return Ok(new { message = "Subcategoría eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
