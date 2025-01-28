using srrf.Dto.Hardware;
using srrf.Models;

namespace srrf.Interfaces
{
    public interface IHardwareRepository
    {
        Task<List<Hardware>> GetAllAsync();
        Task<Hardware?> GetByIdAsync(int id);
        Task<Hardware> CreateAsync(Hardware hardwareModel);
        Task<Hardware?> UpdateAsync(int id, HardwareCUDDto hardwareDto);
        Task<Hardware?> DeleteAsync(int id);
    }
}
