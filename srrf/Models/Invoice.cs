namespace srrf.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string? SupplierName { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }
    }
}
