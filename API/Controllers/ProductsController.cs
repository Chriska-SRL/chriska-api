using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ProductsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_PRODUCTS))]
        public async Task<ActionResult<ProductResponse>> AddProductAsync([FromBody] ProductAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddProductAsync(request);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_PRODUCTS))]
        public async Task<ActionResult<ProductResponse>> UpdateProductAsync(int id, [FromBody] ProductUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateProductAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_PRODUCTS))]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteProductAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int id)
        {
            var result = await _facade.GetProductByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public async Task<ActionResult<List<ProductResponse>>> GetAllProductsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllProductsAsync(options);
            return Ok(result);
        }
    }
}
