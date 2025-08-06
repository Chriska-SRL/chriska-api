using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsZone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleCostsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public VehicleCostsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_VEHICLES))]
        public async Task<ActionResult<VehicleCostResponse>> AddVehicleCostAsync([FromBody] AddVehicleCostRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddVehicleCostAsync(request);
            return Created(String.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_VEHICLES))]
        public async Task<ActionResult<VehicleCostResponse>> UpdateVehicleCostAsync(int id, [FromBody] UpdateVehicleCostRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateVehicleCostAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_VEHICLES))]
        public async Task<IActionResult> DeleteVehicleCostAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteVehicleCostAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public async Task<ActionResult<VehicleCostResponse>> GetByIdAsync(int id)
        {
            var result = await _facade.GetVehicleCostByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public async Task<ActionResult<List<VehicleCostResponse>>> GetAllVehicleCost([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllCosts(options);
            return Ok(result); // 200 OK
        }
      

    }
}
