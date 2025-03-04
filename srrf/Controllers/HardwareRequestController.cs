using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using srrf.Data;
using srrf.Dto.HardwareRequest;
using srrf.Interfaces;
using srrf.Mapper;
using srrf.Models;
using srrf.Queries;
using System.Security.Claims;

namespace srrf.Controllers
{
    //[Authorize]
    [Route("api/HardwareRequest")]
    [ApiController]
    public class HardwareRequestController : ControllerBase
    {
        private readonly Context _context;
        private readonly IHardwareRequestRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<HardwareRequestController> _logger;
        public HardwareRequestController(Context context, 
            IHardwareRequestRepository repository, 
            IHttpContextAccessor contextAccessor,
            ILogger<HardwareRequestController> logger)
        {

            _context = context;
            _repository = repository;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryRequestz query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestHardware = await _repository.GetAllAsync(query);
            var requestHardwaredto = requestHardware.Select(h => h.ToHardwareRequestDto());

            return Ok(requestHardwaredto);
        }

        [Authorize(Roles = "RequestManager")]
        [HttpGet("{hardwareRequestId:int}")]
        public async Task<IActionResult> GetId(int hardwareRequestId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hardwareRequest = await _repository.GetByIdAsync(hardwareRequestId);

            if (hardwareRequest == null)
            {
                return NotFound();
            }

            return Ok(hardwareRequest.ToHardwareRequestDto());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HardwareRequestCUDDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userPrincipal = _contextAccessor.HttpContext?.User;
            var userRoles = userPrincipal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();

            if (!userRoles.Contains("RequestManager"))
            {
                createDto.IsFulfilled = false;
                createDto.SerialNo = "AAA000";
            }

            var hardwareRequestModel = createDto.CreateHardwareRequestDto();
            await _repository.CreateAsync(hardwareRequestModel);

            return CreatedAtAction(nameof(GetId), new { hardwareRequestId = hardwareRequestModel.RequestId }, hardwareRequestModel.ToHardwareRequestDto());
        }

        [Authorize(Roles = "RequestManager, User")]
        [HttpPut]
        [Route("{hardwareRequestId:int}")]
        public async Task<IActionResult> Update([FromRoute] int hardwareRequestId, [FromBody] HardwareRequestCUDDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for update request.");
                return BadRequest(ModelState);
            }

            var userPrincipal = _contextAccessor.HttpContext?.User;
            if (userPrincipal == null || !userPrincipal.Identity.IsAuthenticated)
            {
                _logger.LogWarning("Unauthorized access attempt - user not authenticated.");
                return Unauthorized("User not authenticated.");
            }

            var allClaims = userPrincipal.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            _logger.LogInformation($"All claims: {string.Join(", ", allClaims)}");

            var userName = userPrincipal.FindFirst(ClaimTypes.GivenName)?.Value
                           ?? userPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            var userRoles = userPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            _logger.LogInformation($"User attempting update: {userName}, Roles: {string.Join(", ", userRoles)}, Request ID: {hardwareRequestId}");

            var hardwareRequestModel = await _repository.UpdateAsync(hardwareRequestId, updateDto, userName, userRoles);

            if (hardwareRequestModel == null)
            {
                _logger.LogWarning($"Update failed for Request ID: {hardwareRequestId}. Request not found or unauthorized.");
                return NotFound("Request not found or unauthorized.");
            }

            return Ok(hardwareRequestModel.ToHardwareRequestDto());
        }


        [Authorize(Roles = "RequestManager")]
        [HttpDelete]
        [Route("{hardwareRequestId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int hardwareRequestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hardwareRequestModel = await _repository.DeleteAsync(hardwareRequestId);

            if (hardwareRequestModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
