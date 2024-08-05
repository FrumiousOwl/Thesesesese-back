using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto;
using srrf.Interface;
using srrf.Models;
using srrf.Repository;
using System.Diagnostics.Eventing.Reader;

namespace srrf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly SrrfContext _context;
        private readonly IMapper _mapper;
        private readonly IInvoice _invoiceRepository;
        public InvoiceController(SrrfContext context, IMapper mapper, IInvoice invoiceRepository)
        {
            _context = context;
            _mapper = mapper;
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Invoice>))]
        public IActionResult GetInvoices()
        {
            var invoice = _mapper.Map<List<InvoiceDto>>(_invoiceRepository.GetInvoices());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(invoice);
        }

        [HttpGet("SearchId/{invoiceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetInvoiceById([FromQuery] int invoiceId)
        {
            if (invoiceId == null)
            {

                return NotFound();
            }

            var catId = await _context.Invoices
            .Where(c => c.InvoiceId == invoiceId)
            .ToListAsync();

            var invoiceMap = _mapper.Map<List<InvoiceDto>>(invoiceId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(invoiceMap);
        }

        [HttpGet("SearhByInvoiceNo/{invoiceNo}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetInvoiceByInvoiceNo(int invoiceNo)
        {
            if (invoiceNo == null)
            {
                return NotFound();
            }

            var invoiceNumber = await _context.Invoices
                .Where(i => i.InvoiceNo == invoiceNo)
                .ToListAsync();

            var invoiceMap = await _context.Invoices.FindAsync(invoiceNo);

            return Ok(invoiceMap);
        }

        [HttpGet("searchSupplier/{supplierName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetInvoiceBySupplier([FromQuery] string supplierName)
        {
            if (supplierName == null)
            {
                return NotFound();
            }

            var name = await _context.Invoices
                .Where(c => c.SupplierName.ToUpper().StartsWith(supplierName.ToUpper()))
                .ToListAsync();

            var supplierNameMap = _mapper.Map<List<InvoiceDto>>(name);

            return Ok(supplierNameMap);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateInvoice([FromBody] InvoiceDto invoice)
        {
            if (invoice == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoiceMap = _mapper.Map<Invoice>(invoice);

            if (!_invoiceRepository.CreateInvoice(invoiceMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created invoice");
        }

        [HttpPut("{invoiceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateInvoice(int invoiceId, [FromBody] InvoiceDto updatedInvoice)
        {
            if (updatedInvoice == null)
                return BadRequest(ModelState);

            if (invoiceId != updatedInvoice.InvoiceId)
                return BadRequest(ModelState);

            if (!_invoiceRepository.InvoiceExists(invoiceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var invoiceMap = _mapper.Map<Invoice>(updatedInvoice);

            if (!_invoiceRepository.UpdateInvoice(invoiceMap))
            {
                ModelState.AddModelError("", "Something went wrong updating invoice");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{invoiceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteInvoice(int invoiceId)
        {
            if (!_invoiceRepository.InvoiceExists(invoiceId))
            {
                return NotFound();
            }

            var invoiceToDelete = _invoiceRepository.GetInvoice(invoiceId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_invoiceRepository.DeleteInvoice(invoiceToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting invoice");
            }

            return NoContent();
        }
    }
}
