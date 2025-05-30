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

        [HttpGet]
        public ActionResult<List<RoleResponse>> GetAll()
        {
            var roles = _facade.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public ActionResult<RoleResponse> GetById(int id)
        {
            try
            {
                var role = _facade.GetRoleById(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddRoleRequest request)
        {
            try
            {
                _facade.AddRole(request);
                return Ok(new { message = "Rol creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateRoleRequest request)
        {
            try
            {
                _facade.UpdateRole(request);
                return Ok(new { message = "Rol actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] DeleteRoleRequest request)
        {
            try
            {
                _facade.DeleteRole(request);
                return Ok(new { message = "Rol eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}