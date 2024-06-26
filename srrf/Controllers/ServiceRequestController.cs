using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Models;

namespace srrf.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly SrrfContext _context;
        public ServiceRequestController(SrrfContext context) {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests() {
            if (_context.ServiceRequests == null) {
                return NotFound();
            }
            return await _context.ServiceRequests.ToListAsync();
        }
    }
}
