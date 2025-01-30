using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.Hardware;
using srrf.Interfaces;
using srrf.Mapper;
using srrf.Models;
using srrf.Queries;

namespace srrf.Controllers
{

    [Route("api/Hardware")]
    [ApiController]
    public class HardwareController : ControllerBase
    {
        private readonly Context _context;
        private readonly IHardwareRepository _repository;
        public HardwareController(Context context, IHardwareRepository repository)
        {

            _context = context;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryHardware queryHardware)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var hardwares = await _repository.GetAllAsync(queryHardware);
            var hardwaresdto = hardwares.Select(h => h.ToHardwareDto());

            return Ok(hardwares);
        }

        [HttpGet("{hardwareId:int}")]
        public async Task<IActionResult> GetId(int hardwareId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hardware = await _repository.GetByIdAsync(hardwareId);

            if (hardware == null)
            {
                return NotFound();
            }

            return Ok(hardware.ToHardwareDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HardwareCUDDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hardwareModel = createDto.CreateHardwareDto();
            await _repository.CreateAsync(hardwareModel);

            return CreatedAtAction(nameof(GetId), new { hardwareId = hardwareModel.HardwareId }, hardwareModel.ToHardwareDto());
        }

        [HttpPut]
        [Route("{hardwareId:int}")]
        public async Task<IActionResult> Update([FromRoute] int hardwareId, [FromBody] HardwareCUDDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hardwareModel = await _repository.UpdateAsync(hardwareId, updateDto);

            if (hardwareModel == null)
            {
                return NotFound();
            }

            return Ok(hardwareModel.ToHardwareDto());
        }

        [HttpDelete]
        [Route("{hardwareId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int hardwareId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hardwareModel = await _repository.DeleteAsync(hardwareId);

            if (hardwareModel == null)
            {
                return NotFound();
            }
              
            return NoContent();
        }
    }
}
