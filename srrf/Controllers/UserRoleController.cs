using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using srrf.Dto.Account;
using srrf.Service;

namespace srrf.Controllers
{
    [Route("api/user-role")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly UserRoleService _userRoleService;

        public UserRoleController(UserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var userRoles = await _userRoleService.GetAllUserRolesAsync();
            return Ok(userRoles);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto request)
        {
            var success = await _userRoleService.ChangeUserRoleAsync(request.UserId, request.NewRole);
            if (!success) return BadRequest("Failed to update role.");
            return Ok("User role updated successfully.");
        }

    }
}
