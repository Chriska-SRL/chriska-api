using BusinessLogic;
using BusinessLogic.DTOs.DTOsUser;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Facade _facade;

        public UsersController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] AddUserRequest request)
        {
            try
            {
                _facade.AddUser(request);
                return Ok(new { message = "Usuario agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                _facade.UpdateUser(request);
                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromBody] DeleteUserRequest request)
        {
            try
            {
                _facade.DeleteUser(request);
                return Ok(new { message = "Usuario eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetUserById(int id)
        {
            try
            {
                var response = _facade.GetUserById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<UserResponse>> GetAllUsers()
        {
            try
            {
                var response = _facade.GetAllUsers();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
