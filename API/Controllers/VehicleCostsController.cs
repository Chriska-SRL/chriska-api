using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleCostsController : ControllerBase
    {
        private readonly Facade _facade;

        public VehicleCostsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_VEHICLES))]
        public IActionResult Add([FromBody] AddVehicleCostRequest request)
        {
            try
            {
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
        public IActionResult Update([FromBody] UpdateVehicleCostRequest request)
        {
            try
            {
                return Ok(_facade.UpdateVehicleCost(request));
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

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_VEHICLES))]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_facade.DeleteVehicleCost(id));
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
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar el costo." });
            }
        }

        [HttpGet("vehicle/{vehicleId}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetAllForVehicle(int vehicleId)
        {
            try
            {
                return Ok(_facade.GetVehicleCosts(vehicleId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al obtener los costos del vehículo." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_facade.GetVehicleCostById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener el costo con id {id}." });
            }
        }

        [HttpGet("vehicle/{vehicleId}/rango")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetCostsInRange(int vehicleId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                return Ok(_facade.GetVehicleCostsByDateRange(vehicleId, from, to));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener los costos del vehículo {vehicleId}." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
