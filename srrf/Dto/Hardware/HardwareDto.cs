using srrf.Dto.HardwareRequest;
using srrf.Service;

namespace srrf.Dto.Hardware
{
    public class HardwareDto
    {
        public int HardwareId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DatePurchased { get; set; }

        private string _supplier;
        public string Supplier
        {
            get => IsInventoryManager ? AesEncryption.Decrypt(_supplier) : _supplier; // Decrypt for Inventory Managers
            set => _supplier = value; // Store the encrypted value directly from DB
        }

        private string _totalPrice;
        public string TotalPrice
        {
            get => IsInventoryManager ? AesEncryption.Decrypt(_totalPrice) : _totalPrice;
            set => _totalPrice = value; 
        }

        public int? Defective { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
        public bool IsInventoryManager { get; set; }
    }

}
