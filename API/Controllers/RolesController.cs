using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsRole;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly Facade _facade;

        public RolesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddRole([FromBody] AddRoleRequest request)
        {
            try
            {
                _facade.AddRole(request);
                return Ok(new { message = "Rol agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateRole([FromBody] UpdateRoleRequest request)
        {
            try
            {
                _facade.UpdateRole(request);
                return Ok(new { message = "Rol actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteRole([FromBody] DeleteRoleRequest request)
        {
            try
            {
                _facade.DeleteRole(request);
                return Ok(new { message = "Rol eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
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
