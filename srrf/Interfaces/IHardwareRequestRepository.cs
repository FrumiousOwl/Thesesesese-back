using srrf.Dto.HardwareRequest;
using srrf.Models;
using srrf.Queries;

namespace srrf.Interfaces
{
    public interface IHardwareRequestRepository
    {
        Task<List<HardwareRequest>> GetAllAsync(QueryRequestz query);
        Task<HardwareRequest?> GetByIdAsync(int id);
        Task<HardwareRequest> CreateAsync(HardwareRequest hardwareRequestModel);
        Task<HardwareRequest?> UpdateAsync(int id, HardwareRequestCUDDto hardwareRequestDto);
        Task<HardwareRequest?> DeleteAsync(int id);
    }
}
