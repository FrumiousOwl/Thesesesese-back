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
        [Authorize]
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

/*        [HttpGet("available/{hardwareId}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetHardwareAvaialbleStatus(int hardwareId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hardware = await _context.Hardware.FindAsync(hardwareId);

            if (hardware == null)
            {
                return NotFound();
            }

            var hardwareStatusDto = new AvailableHardwareDto
            {
                HardwareId = hardware.HardwareId,
                Name = hardware.Name,
                Supplier = hardware.Supplier,
                DatePurchased = hardware.DatePurchased,
                Available = hardware.Defective,
                Deployed = hardware.Deployed
            };

            return Ok(hardwareStatusDto);
        }

        [HttpGet("defective/{hardwareId}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetHardwareDefectiveStatus(int hardwareId)
        {
            var hardware = await _context.Hardware.FindAsync(hardwareId);

            if (hardware == null)
            {
                return NotFound();
            }

            return Ok(hardware.Defective);
        }*/

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
        [Authorize]
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
        [Authorize]
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
