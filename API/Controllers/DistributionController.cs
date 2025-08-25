using API.Utils;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.DTOs.DTOsDistribution;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DistributionController : Controller
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public DistributionController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_DISTRIBUTIONS))]
        public async Task<ActionResult<DistributionResponse>> AddDistributionAsync([FromBody] DistributionAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddDistributionAsync(request);
            return Created(String.Empty, result); // 201 Created con Location
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_DISTRIBUTIONS))]
        public async Task<ActionResult<DistributionResponse>> UpdateDistributionAsync(int id, [FromBody] DistributionUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateDistributionAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_DISTRIBUTIONS))]
        public async Task<IActionResult> DeleteDistributionAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteDistributionAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_DISTRIBUTIONS))]
        public async Task<ActionResult<DistributionResponse>> GetDistributionByIdAsync(int id)
        {
            var result = await _facade.GetDistributionByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_DISTRIBUTIONS))]
        public async Task<ActionResult<List<DistributionResponse>>> GetAllDistributionsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllDistributionsAsync(options);
            return Ok(result); // 200 OK
        }
    }
}

