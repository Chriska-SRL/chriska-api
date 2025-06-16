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
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener las categorías." });
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
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener la categoría con id {id}." });
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_CATEGORIES))]
        public ActionResult<CategoryResponse> AddCategory([FromBody] AddCategoryRequest request)
        {
            try
            {
                return Ok(_facade.AddCategory(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al agregar la categoría." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_CATEGORIES))]
        public ActionResult<CategoryResponse> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            try
            {
                return Ok(_facade.UpdateCategory(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar la categoría." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CATEGORIES))]
        public ActionResult<CategoryResponse> DeleteCategory(int id)
        {
            try
            {
                return Ok(_facade.DeleteCategory(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar la categoría." });
            }
        }

        // SubCategories

        [HttpGet("subcategories")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public ActionResult<List<SubCategoryResponse>> GetAllSubCategories()
        {
            try
            {
                return Ok(_facade.GetAllSubCategories());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener las subcategorías." });
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
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener la subcategoría con id {id}." });
            }
        }

        [HttpPost("subcategories")]
        [Authorize(Policy = nameof(Permission.CREATE_CATEGORIES))]
        public ActionResult<SubCategoryResponse> AddSubCategory([FromBody] AddSubCategoryRequest request)
        {
            try
            {
                return Ok(_facade.AddSubCategory(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al agregar la subcategoría." });
            }
        }

        [HttpPut("subcategories")]
        [Authorize(Policy = nameof(Permission.EDIT_CATEGORIES))]
        public ActionResult<SubCategoryResponse> UpdateSubCategory([FromBody] UpdateSubCategoryRequest request)
        {
            try
            {
                return Ok(_facade.UpdateSubCategory(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar la subcategoría." });
            }
        }

        [HttpDelete("subcategories/{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CATEGORIES))]
        public ActionResult<SubCategoryResponse> DeleteSubCategory(int id)
        {
            try
            {
                return Ok(_facade.DeleteSubCategory(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar la subcategoría." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
