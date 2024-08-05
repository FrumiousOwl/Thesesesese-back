using srrf.Models;

namespace srrf.Interface
{
    public interface IInvoice
    {
        ICollection<Invoice> GetInvoices();
        Invoice GetInvoice(int id);
        bool InvoiceExists(int id);
        bool CreateInvoice(Invoice invoice);
        bool UpdateInvoice(Invoice invoice);
        bool DeleteInvoice(Invoice invoice);
        bool Save();
    }
}
