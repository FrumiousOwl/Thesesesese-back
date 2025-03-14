using srrf.Dto.Hardware;
using srrf.Models;
using srrf.Queries;

namespace srrf.Interfaces
{
    public interface IHardwareRepository
    {
        Task<List<Hardware>> GetAllAsync(QueryHardware queryHardware);
        Task<Hardware?> GetByIdAsync(int id);
        Task<Hardware> CreateAsync(Hardware hardwareModel);
        Task<Hardware?> UpdateAsync(int id, HardwareCUDDto hardwareDto);
        Task<Hardware?> DeleteAsync(int id);
        Task<IEnumerable<AvailableHardwareDto>> GetAvailableHardwareAsync(QueryAvailableHardware query, bool isInventoryManager);
        Task<IEnumerable<DefectiveHardwareDto>> GetDefectiveHardwareAsync(QueryDefectiveHardware query, bool isInventoryManager);

    }
}
