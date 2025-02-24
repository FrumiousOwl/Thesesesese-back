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
        //[Authorize]
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
        //[Authorize]
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

        [HttpGet("available/getAllAvailableHardware")]
        //[Authorize]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetHardwareAvailableStatus()
        {
            try
            {
                var availableHardware = await _context.Hardware
                    .Where(h => h.Available > 0) 
                    .Select(h => new AvailableHardwareDto
                    {
                        HardwareId = h.HardwareId,
                        Name = h.Name,
                        Available = h.Available,
                        Deployed = h.Deployed,
                        DatePurchased = h.DatePurchased,
                        Supplier = h.Supplier
                    })
                    .ToListAsync();

                return Ok(availableHardware);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("defective/getAllDefectiveHardware")]
        //[Authorize]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetHardwareDefectiveStatus()
        {
            try
            {
                var defectiveHardware = await _context.Hardware
                    .Where(h => h.Defective > 0)
                    .Select(h => new DefectiveHardwareDto
                    {
                        HardwareId = h.HardwareId,
                        Name = h.Name,
                        Defective = h.Defective,
                        DatePurchased = h.DatePurchased,
                        Supplier = h.Supplier
                    })
                    .ToListAsync();

                return Ok(defectiveHardware);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        [HttpPost]
        //[Authorize]
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
        //[Authorize]
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
        //[Authorize]
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
