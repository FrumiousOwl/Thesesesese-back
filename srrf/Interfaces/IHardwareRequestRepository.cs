using srrf.Dto.HardwareRequest;
using srrf.Models;
using srrf.Queries;

namespace srrf.Interfaces
{
    public interface IHardwareRequestRepository
    {
        Task<List<HardwareRequest>> GetAllAsync(QueryRequestz query);
        Task<HardwareRequest?> GetByIdAsync(int id);
        Task<IEnumerable<HardwareRequest?>> GetRequestsByUserAsync(string name);
        Task<HardwareRequest> CreateAsync(HardwareRequest hardwareRequestModel);
        Task<HardwareRequest?> UpdateAsync(int id, HardwareRequestCUDDto hardwareRequestDto, string userName, List<string> userRoles);
        Task<HardwareRequest?> DeleteAsync(int id);
    }
}
