using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsImage;
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
            return Created(String.Empty, result);
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

        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = nameof(Permission.EDIT_PRODUCTS))]
        public async Task<IActionResult> UploadProductImageAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("El archivo es inválido o está vacío.");
            AddImageRequest request = new AddImageRequest
            {
                EntityId = id,
                File = file,
            };
            request.setUserId(_tokenUtils.GetUserId());
            string url = await _facade.UploadProductImageAsync(request);
            return Ok(url);
        }

        [HttpPost("{id}/delete-image")]
        [Authorize(Policy = nameof(Permission.EDIT_PRODUCTS))]
        public async Task<IActionResult> DeleteProductImageAsync(int id)
        {
            await _facade.DeleteProductImageAsync(id);
            return NoContent();
        }
    }
}
