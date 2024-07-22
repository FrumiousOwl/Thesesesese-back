using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto;
using srrf.Interface;
using srrf.Models;
using srrf.Repository;
using System.ComponentModel.DataAnnotations;

namespace srrf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequest _serviceRequest;
        private readonly IMapper _mapper;
        private readonly SrrfContext _context;
        public ServiceRequestController(SrrfContext context, IServiceRequest serviceRequest, IMapper mapper)
        {
            _context = context;
            _serviceRequest = serviceRequest;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequest>))]
        public IActionResult GetServiceRequests()
        {
            var serviceRequest = _mapper.Map<List<ServiceRequestDto>>(_serviceRequest.GetServiceRequests());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(serviceRequest);
        }

        [HttpGet("{serviceRequestId}")]
        [ProducesResponseType(200, Type = typeof(ServiceRequest))]
        [ProducesResponseType(400)]
        public IActionResult GetServiceRequest(int serviceRequestId)
        {
            if (!_serviceRequest.ServiceRequestExists(serviceRequestId))
                return NotFound();

            var serviceRequest = _mapper.Map<ServiceRequestDto>(_serviceRequest.GetServiceRequest(serviceRequestId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(serviceRequest);
        }

        [HttpGet("{requestsName}/searchName")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRequestsByName([FromQuery] string name)
        {
            var requests = await _context.ServiceRequests
                .Where(n => n.Name.ToLower().StartsWith(name.ToLower()))
                .ToListAsync();

            var requestes = _mapper.Map<List<ServiceRequestDto>>(requests);

            return Ok(requestes);
        }

        [HttpGet("{requestsDates}/requestDate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRequestsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var requests = await _context.ServiceRequests
                .Where(r => r.DateNeeded >= startDate && r.DateNeeded <= endDate)
                .ToListAsync();

            var requestes = _mapper.Map<List<ServiceRequestDto>>(requests);

            return Ok(requestes);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRequest([FromBody] ServiceRequestDto serviceRequest)
        {
            if (serviceRequest == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (serviceRequest.CategoryId == null && serviceRequest.Name == null)
            {
                throw new ArgumentNullException(nameof(serviceRequest.CategoryId), " Name is required.");
            }

            var serviceRequestMap = _mapper.Map<ServiceRequest>(serviceRequest);

            if (!_serviceRequest.CreateRequest(serviceRequestMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{requestId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRequest(int requestId, [FromBody] ServiceRequestDto updatedServiceRequest)
        {
            if (updatedServiceRequest == null)
                return BadRequest(ModelState);

            if (requestId != updatedServiceRequest.Id)
                return BadRequest(ModelState);

            if (!_serviceRequest.ServiceRequestExists(requestId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var serviceRequestMap = _mapper.Map<ServiceRequest>(updatedServiceRequest);

            if (!_serviceRequest.UpdateRequest(serviceRequestMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{requestId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRequest(int requestId)
        {
            if (!_serviceRequest.ServiceRequestExists(requestId))
            {
                return NotFound();
            }

            var serviceRequestToDelete = _serviceRequest.GetServiceRequest(requestId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_serviceRequest.DeleteRequest(serviceRequestToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting");
            }

            return NoContent();
        }
    }
}
