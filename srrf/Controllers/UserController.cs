using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.HardwareRequest;
using srrf.Dto.User;
using srrf.Interfaces;
using srrf.Mapper;
using srrf.Models;
using srrf.Queries;

namespace srrf.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _context;
        private readonly IUserRepository _repository;
        public UserController(Context context, IUserRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryUser queryUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _repository.GetAllAsync(queryUser);
            var userDto = users.Select(x => x.ToUserDto());

            return Ok(users);
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetId(int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _repository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCRUD createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userModel = createDto.CreateUserDto();
            await _repository.CreateAsync(userModel);

            return CreatedAtAction(nameof(GetId), new { userId = userModel.UserId }, userModel.ToUserDto());
        }

        [HttpPut]
        [Route("{userId:int}")]
        public async Task<IActionResult> Update([FromRoute] int userId, [FromBody] UserCRUD updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userModel = await _repository.UpdateAsync(userId, updateDto);

            if (userModel == null)
            {
                return NotFound(userId);
            }

            return Ok(userModel.ToUserDto());
        }

        [HttpDelete]
        [Route("{userId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hardwareRequestModel = await _repository.DeleteAsync(userId);

            if (hardwareRequestModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
