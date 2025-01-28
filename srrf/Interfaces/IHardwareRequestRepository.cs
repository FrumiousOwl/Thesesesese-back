using srrf.Dto.HardwareRequest;
using srrf.Models;

namespace srrf.Interfaces
{
    public interface IHardwareRequestRepository
    {
        Task<List<HardwareRequest>> GetAllAsync();
        Task<HardwareRequest?> GetByIdAsync(int id);
        Task<HardwareRequest> CreateAsync(HardwareRequest hardwareRequestModel);
        Task<HardwareRequest?> UpdateAsync(int id, HardwareRequestCUDDto hardwareRequestDto);
        Task<HardwareRequest?> DeleteAsync(int id);
    }
}
