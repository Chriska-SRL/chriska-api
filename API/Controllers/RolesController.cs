using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsRole;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Aplica autenticación por defecto
    public class RolesController : ControllerBase
    {
        private readonly Facade _facade;

        public RolesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_ROLES))]
        public IActionResult AddRole([FromBody] AddRoleRequest request)
        {
            try
            {
                var result = _facade.AddRole(request);
                return Ok(result);
            }
            catch (ArgumentException ex) 
            {
                var mensaje = ex.Message.Split(" (Parameter")[0];
                return BadRequest(new { campo = ex.ParamName, error = mensaje });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar el rol." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_ROLES))]
        public IActionResult UpdateRole([FromBody] UpdateRoleRequest request)
        {
            try
            {
                return Ok(_facade.UpdateRole(request));
            }
            catch (ArgumentException ex)
            {
                var mensaje = ex.Message.Split(" (Parameter")[0];
                return BadRequest(new { campo = ex.ParamName, error = mensaje });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el rol." });
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(Permission.DELETE_ROLES))]
        public IActionResult DeleteRole([FromBody] DeleteRoleRequest request)
        {
            try
            {
                return Ok(_facade.DeleteRole(request));
            }
            catch (ArgumentException ex)
            {
                var mensaje = ex.Message.Split(" (Parameter")[0];
                return BadRequest(new { campo = ex.ParamName, error = mensaje });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el rol." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_ROLES))]
        public ActionResult<RoleResponse> GetRoleById(int id)
        {
            try
            {
                var response = _facade.GetRoleById(id);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                var mensaje = ex.Message.Split(" (Parameter")[0];
                return BadRequest(new { campo = ex.ParamName, error = mensaje });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el rol." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_ROLES))]
        public ActionResult<List<RoleResponse>> GetAllRoles()
        {
            try
            {
                var response = _facade.GetAllRoles();
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                var mensaje = ex.Message.Split(" (Parameter")[0];
                return BadRequest(new { campo = ex.ParamName, error = mensaje });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el rol." });
            }
        }
    }
}
