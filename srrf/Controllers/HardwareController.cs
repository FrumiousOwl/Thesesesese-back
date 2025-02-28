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
using srrf.Repository;

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

        [Authorize(Roles = "InventoryManager, RequestManager, SystemManager")]
        [HttpGet]
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

        [Authorize(Roles = "InventoryManager, RequestManager, SystemManager")]
        [HttpGet("available/getAllAvailableHardware")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAvailableHardware([FromQuery] QueryAvailableHardware query)
        {
            try
            {
                var availableHardware = await _repository.GetAvailableHardwareAsync(query);
                return Ok(availableHardware);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "InventoryManager, RequestManager, SystemManager")]
        [HttpGet("defective/getAllDefectiveHardware")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDefectiveHardware([FromQuery] QueryDefectiveHardware query)
        {
            try
            {
                var defectiveHardware = await _repository.GetDefectiveHardwareAsync(query);
                return Ok(defectiveHardware);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "InventoryManager")]
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
        [Authorize(Roles = "InventoryManager")]
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

        [Authorize(Roles = "InventoryManager")]
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
