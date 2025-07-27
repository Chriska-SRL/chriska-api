using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public UsersController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_USERS))]
        public async Task<ActionResult<UserResponse>> AddUserAsync([FromBody] AddUserRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddUserAsync(request);
            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_USERS))]
        public async Task<ActionResult<UserResponse>> UpdateUserAsync(int id, [FromBody] UpdateUserRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateUserAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_USERS))]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteUserAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_USERS))]
        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(int id)
        {
            var result = await _facade.GetUserByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_USERS))]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsersAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllUsersAsync(options);
            return Ok(result);
        }

        [HttpPost("resetpassword")]
        [Authorize(Policy = nameof(Permission.EDIT_USERS))]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            await _facade.ResetPasswordAsync(request.UserId, request.NewPassword);
            return Ok(new { message = "Contraseña restablecida correctamente" });
        }

        [HttpPost("resetmypassword")]
        public async Task<IActionResult> ResetMyPasswordAsync([FromBody] ResetMyPasswordRequest request)
        {
            var user = await _facade.AuthenticateAsync(request.Username, request.OldPassword);
            if (user == null)
                return Unauthorized(new { error = "Credenciales inválidas" });

            await _facade.ResetPasswordAsync(user.Id, request.NewPassword);
            return Ok(new { message = "Contraseña restablecida correctamente" });
        }
    }
}
