using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsSupplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public SuppliersController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_SUPPLIERS))]
        public async Task<ActionResult<SupplierResponse>> AddSupplierAsync([FromBody] AddSupplierRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddSupplierAsync(request);
            return Created(String.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_SUPPLIERS))]
        public async Task<ActionResult<SupplierResponse>> UpdateSupplierAsync(int id, [FromBody] UpdateSupplierRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateSupplierAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_SUPPLIERS))]
        public async Task<IActionResult> DeleteSupplierAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteSupplierAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_SUPPLIERS))]
        public async Task<ActionResult<SupplierResponse>> GetSupplierByIdAsync(int id)
        {
            var result = await _facade.GetSupplierByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_SUPPLIERS))]
        public async Task<ActionResult<List<SupplierResponse>>> GetAllSuppliersAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllSuppliersAsync(options);
            return Ok(result);
        }
    }
}
