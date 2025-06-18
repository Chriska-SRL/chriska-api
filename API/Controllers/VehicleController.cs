using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsVehicle;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly Facade _facade;

        public VehiclesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_VEHICLES))]
        public IActionResult AddVehicle([FromBody] AddVehicleRequest request)
        {
            try
            {
                var result = _facade.AddVehicle(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar el vehículo." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_VEHICLES))]
        public IActionResult UpdateVehicle([FromBody] UpdateVehicleRequest request)
        {
            try
            {
                return Ok(_facade.UpdateVehicle(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el vehículo." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_VEHICLES))]
        public IActionResult DeleteVehicle(int id)
        {
            try
            {
                return Ok(_facade.DeleteVehicle(id));
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
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar el vehículo." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetVehicleById(int id)
        {
            try
            {
                return Ok(_facade.GetVehicleById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener el vehículo con id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetAllVehicles()
        {
            try
            {
                return Ok(_facade.GetAllVehicles());
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los vehículos." });
            }
        }

        [HttpGet("matricula/{plate}")]
        [Authorize(Policy = nameof(Permission.VIEW_VEHICLES))]
        public IActionResult GetVehicleByPlate(string plate)
        {
            try
            {
                return Ok(_facade.GetVehicleByPlate(plate));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener el vehículo con matrícula {plate}." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
