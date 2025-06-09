using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsUser;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly Facade _facade;

        public UsersController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_USERS))]
        public IActionResult AddUser([FromBody] AddUserRequest request)
        {
            try
            {
                return Ok(_facade.AddUser(request));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_USERS))]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                return Ok(_facade.UpdateUser(request));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(Permission.DELETE_USERS))]
        public IActionResult DeleteUser([FromBody] DeleteUserRequest request)
        {
            try
            {
                return Ok(_facade.DeleteUser(request));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_USERS))]
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
        [Authorize(Policy = nameof(Permission.VIEW_USERS))]
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
