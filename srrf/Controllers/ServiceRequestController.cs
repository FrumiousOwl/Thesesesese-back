using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto;
using srrf.Interface;
using srrf.Models;
using srrf.Repository;

namespace srrf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequest _serviceRequest;
        private readonly IMapper _mapper;
        public ServiceRequestController(IServiceRequest serviceRequest, IMapper mapper) {
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

        [HttpGet("{serviceRequesterName}")]
        [ProducesResponseType(200, Type = typeof(ServiceRequest))]
        [ProducesResponseType(404)] // Updated response type for not found
        public IActionResult GetServiceRequestByName(string serviceRequesterName)
        {
            if (string.IsNullOrEmpty(serviceRequesterName))
            {
                return BadRequest("Service requester name is required."); // Informative message
            }

            var serviceRequest = _mapper.Map<ServiceRequestDto>(_serviceRequest.GetServiceRequestByName(serviceRequesterName));

            if (serviceRequest == null) // Check if serviceRequest is null after retrieval
            {
                return NotFound("Service request not found for the provided name."); // Informative message
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(serviceRequest);
        }


    }
}
