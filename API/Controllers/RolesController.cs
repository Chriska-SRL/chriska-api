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
                _facade.AddRole(request);
                return Ok(new { message = "Rol agregado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_ROLES))]
        public IActionResult UpdateRole([FromBody] UpdateRoleRequest request)
        {
            try
            {
                _facade.UpdateRole(request);
                return Ok(new { message = "Rol actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(Permission.DELETE_ROLES))]
        public IActionResult DeleteRole([FromBody] DeleteRoleRequest request)
        {
            try
            {
                _facade.DeleteRole(request);
                return Ok(new { message = "Rol eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
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
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
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
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
