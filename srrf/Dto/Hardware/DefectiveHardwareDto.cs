using srrf.Service;

namespace srrf.Dto.Hardware
{
    public class DefectiveHardwareDto
    {
        public int HardwareId { get; set; }
        public string? Name { get; set; }
        public int? Defective { get; set; }
        public DateTime? DatePurchased { get; set; }
        private string _supplier;
        public string Supplier
        {
            get => IsInventoryManager ? AesEncryption.Decrypt(_supplier) : _supplier; 
            set => _supplier = value; 
        }
         public bool IsInventoryManager { get; set; }
    }
}
