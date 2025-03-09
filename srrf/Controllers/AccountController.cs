using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using srrf.Dto.Account;
using srrf.Interfaces;
using srrf.Models;
using System.Security.Claims;
using System.Text;

namespace srrf.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public AccountController(UserManager<User> userManager,
            ITokenService tokenService,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContext) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _contextAccessor = httpContext;
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userPrincipal = _contextAccessor.HttpContext?.User;
            var userEmail = userPrincipal?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User information is missing.");
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                PhoneNumber = registerDto.PhoneNumber,
                                Token = _tokenService.CreateToken(appUser)
                            }
                            );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [Authorize(Roles = "SystemManager")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> AdminResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userPrincipal = _contextAccessor.HttpContext?.User;
            var adminEmail = userPrincipal?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(adminEmail))
            {
                return Unauthorized("SystemManager not found.");
            }

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null || !await _userManager.IsInRoleAsync(adminUser, "SystemManager"))
            {
                return Unauthorized("You are not authorized to perform this action.");
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.UserEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var temporaryPassword = string.IsNullOrEmpty(resetPasswordDto.TemporaryPassword)
                ? GenerateTemporaryPassword()
                : resetPasswordDto.TemporaryPassword;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, temporaryPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password reset successfully.");
        }

        private string GenerateTemporaryPassword()
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            var random = new Random();
            var password = new StringBuilder();
            for (int i = 0; i < 12; i++) 
            {
                password.Append(validChars[random.Next(validChars.Length)]);
            }
            return password.ToString();
        }
    }
}
