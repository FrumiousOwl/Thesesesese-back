using srrf.Service;

namespace srrf.Dto.Hardware
{
    public class AvailableHardwareDto
    {
        public int HardwareId { get; set; }
        public string? Name { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
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