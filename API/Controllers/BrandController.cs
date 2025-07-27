using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsBrand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public BrandsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_BRANDS))]
        public async Task<ActionResult<BrandResponse>> AddBrandAsync([FromBody] AddBrandRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddBrandAsync(request);
            return CreatedAtAction(nameof(GetBrandByIdAsync), new { id = result.Id }, result); // 201 Created con Location
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_BRANDS))]
        public async Task<ActionResult<BrandResponse>> UpdateBrandAsync(int id, [FromBody] UpdateBrandRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateBrandAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_BRANDS))]
        public async Task<IActionResult> DeleteBrandAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteBrandAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_BRANDS))]
        public async Task<ActionResult<BrandResponse>> GetBrandByIdAsync(int id)
        {
            var result = await _facade.GetBrandByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_BRANDS))]
        public async Task<ActionResult<List<BrandResponse>>> GetAllBrandsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllBrandsAsync(options);
            return Ok(result); // 200 OK
        }
    }
}
