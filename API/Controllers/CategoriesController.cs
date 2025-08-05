using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsSubCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public CategoriesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_CATEGORIES))]
        public async Task<ActionResult<CategoryResponse>> AddCategoryAsync([FromBody] AddCategoryRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddCategoryAsync(request);
            return Created(String.Empty, result); // 201 Created con Location
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_CATEGORIES))]
        public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateCategoryAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CATEGORIES))]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteCategoryAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public async Task<ActionResult<CategoryResponse>> GetCategoryByIdAsync(int id)
        {
            var result = await _facade.GetCategoryByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public async Task<ActionResult<List<CategoryResponse>>> GetAllCategoriesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllCategoriesAsync(options);
            return Ok(result);
        }

        // Subcategories

        [HttpPost("subcategories")]
        [Authorize(Policy = nameof(Permission.CREATE_CATEGORIES))]
        public async Task<ActionResult<SubCategoryResponse>> AddSubCategoryAsync([FromBody] AddSubCategoryRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddSubCategoryAsync(request);
            return Created(String.Empty, result); // 201 Created con Location
        }

        [HttpPut("subcategories/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_CATEGORIES))]
        public async Task<ActionResult<SubCategoryResponse>> UpdateSubCategoryAsync(int id, [FromBody] UpdateSubCategoryRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateSubCategoryAsync(request);
            return Ok(result);
        }

        [HttpDelete("subcategories/{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CATEGORIES))]
        public async Task<IActionResult> DeleteSubCategoryAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteSubCategoryAsync(request);
            return NoContent();
        }

        [HttpGet("subcategories/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public async Task<ActionResult<SubCategoryResponse>> GetSubCategoryByIdAsync(int id)
        {
            var result = await _facade.GetSubCategoryByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("subcategories")]
        [Authorize(Policy = nameof(Permission.VIEW_CATEGORIES))]
        public async Task<ActionResult<List<SubCategoryResponse>>> GetAllSubCategoriesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllSubCategoriesAsync(options);
            return Ok(result);
        }
    }
}
