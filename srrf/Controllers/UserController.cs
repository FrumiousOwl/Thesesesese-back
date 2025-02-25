using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using srrf.Dto.Account;
using srrf.Interfaces;
using srrf.Models;
using srrf.Queries;
using srrf.Service;

namespace srrf.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetAllUsers([FromQuery] QueryUser query)
        {
            var users = await _userRepository.GetAllUsersAsync(query);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsersDto>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUsersDto updateUser)
        {
            var success = await _userRepository.UpdateUserAsync(id, updateUser);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteUser(string id)
        {
            var success = await _userRepository.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
