using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.HardwareRequest;
using srrf.Interfaces;
using srrf.Mapper;
using srrf.Models;
using srrf.Queries;

namespace srrf.Controllers
{
    //[Authorize]
    [Route("api/HardwareRequest")]
    [ApiController]
    public class HardwareRequestController : ControllerBase
    {
        private readonly Context _context;
        private readonly IHardwareRequestRepository _repository;
        public HardwareRequestController(Context context, IHardwareRequestRepository repository)
        {

            _context = context;
            _repository = repository;
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

            var hardwareRequestModel = createDto.CreateHardwareRequestDto();
            await _repository.CreateAsync(hardwareRequestModel);

            return CreatedAtAction(nameof(GetId), new { hardwareRequestId = hardwareRequestModel.RequestId }, hardwareRequestModel.ToHardwareRequestDto());
        }

        [Authorize(Roles = "RequestManager")]
        [HttpPut]
        [Route("{hardwareRequestId:int}")]
        public async Task<IActionResult> Update([FromRoute] int hardwareRequestId, [FromBody] HardwareRequestCUDDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hardwareRequestModel = await _repository.UpdateAsync(hardwareRequestId, updateDto);

            if (hardwareRequestModel == null)
            {
                return NotFound(hardwareRequestId);
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
