using srrf.Data;
using srrf.Interface;
using srrf.Models;

namespace srrf.Repository
{
    public class InvoiceRepository : IInvoice
    {
        private readonly SrrfContext _context;
        public InvoiceRepository(SrrfContext context) 
        {
            _context = context;
        }
        public bool CreateInvoice(Invoice invoice)
        {
            _context.Add(invoice);
            return Save();
        }

        public bool DeleteInvoice(Invoice invoice)
        {
            _context.Remove(invoice);
            return Save();
        }

        public Invoice GetInvoice(int id)
        {
            return _context.Invoices.Where(i => i.InvoiceId == id).FirstOrDefault();
        }

        public ICollection<Invoice> GetInvoices()
        {
            return _context.Invoices.OrderBy(i => i.InvoiceId).ToList();
        }

        public bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(i => i.InvoiceId == id);
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateInvoice(Invoice invoice)
        {
            _context.Update(invoice);
            return Save();
        }
    }
}
