using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;

namespace API.Controllers
{
    [ApiController]
    [Route("api/vehicles/{vehicleId}/costs")]
    [Authorize]
    public class VehicleCostsController : ControllerBase
    {
        private readonly Facade _facade;

        public VehicleCostsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.EDIT_VEHICLES))]
        public IActionResult AddVehicleCost(int vehicleId, [FromBody] AddVehicleCostRequest request)
        {
            try
            {
                request.VehicleId = vehicleId;
                var result = _facade.AddVehicleCost(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar el costo." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_VEHICLES))]
        public IActionResult UpdateVehicleCost(int vehicleId, [FromBody] UpdateVehicleCostRequest request)
        {
            try
            {
                request.VehicleId = vehicleId;
                var result = _facade.UpdateVehicleCost(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el costo." });
            }
        }

        [HttpDelete("{costId}")]
        [Authorize(Policy = nameof(Permission.EDIT_VEHICLES))]
        public IActionResult DeleteVehicleCost(int vehicleId, int costId)
        {
            try
            {
                var result = _facade.DeleteVehicleCost(vehicleId, costId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar el costo." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetVehicleCosts(int vehicleId)
        {
            try
            {
                var result = _facade.GetVehicleCosts(vehicleId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los costos." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
