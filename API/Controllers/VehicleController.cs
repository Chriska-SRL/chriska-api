using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsVehicle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public VehiclesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_VEHICLES))]
        public async Task<ActionResult<VehicleResponse>> AddVehicleAsync([FromBody] AddVehicleRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddVehicleAsync(request);
            return CreatedAtAction(nameof(GetVehicleByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_VEHICLES))]
        public async Task<ActionResult<VehicleResponse>> UpdateVehicleAsync(int id, [FromBody] UpdateVehicleRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateVehicleAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_VEHICLES))]
        public async Task<IActionResult> DeleteVehicleAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteVehicleAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public async Task<ActionResult<VehicleResponse>> GetVehicleByIdAsync(int id)
        {
            var result = await _facade.GetVehicleByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public async Task<ActionResult<List<VehicleResponse>>> GetAllVehiclesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllVehiclesAsync(options);
            return Ok(result);
        }

        [HttpGet("matricula/{plate}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public async Task<ActionResult<VehicleResponse>> GetVehicleByPlateAsync(string plate)
        {
            var result = await _facade.GetVehicleByPlateAsync(plate);
            return Ok(result);
        }
    }
}
