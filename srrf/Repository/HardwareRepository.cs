using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.Hardware;
using srrf.Interfaces;
using srrf.Models;

namespace srrf.Repository
{
    public class HardwareRepository : IHardwareRepository
    {

        private readonly Context _context;
        public HardwareRepository(Context context)
        {
            _context = context;
        }

        public async Task<Hardware> CreateAsync(Hardware hardwareModel)
        {
            await _context.Hardware.AddAsync(hardwareModel);
            await _context.SaveChangesAsync();
            return hardwareModel;
        }

        public async Task<Hardware?> DeleteAsync(int id)
        {
            var hardwareModel = await _context.Hardware.FirstOrDefaultAsync(x => x.HardwareId == id);
            
            if (hardwareModel == null)
            {
                return null;
            }

            _context.Hardware.Remove(hardwareModel);
            await _context.SaveChangesAsync();
            return hardwareModel;
        }

        public async Task<List<Hardware>> GetAllAsync()
        {
            return await _context.Hardware.ToListAsync();
        }

        public async Task<Hardware?> GetByIdAsync(int id)
        {
            return await _context.Hardware.FindAsync(id);
        }

        public async Task<Hardware?> UpdateAsync(int id, HardwareCUDDto hardwareDto)
        {
            var existHardware = await _context.Hardware.FirstOrDefaultAsync(x =>x.HardwareId == id);

            if (existHardware == null)
            {
                return null;
            }

            existHardware.Name = hardwareDto.Name;
            existHardware.Description = hardwareDto.Description;
            existHardware.DatePurchased = hardwareDto.DatePurchased;
            existHardware.Supplier = hardwareDto.Supplier;
            existHardware.Defective = hardwareDto.Defective;
            existHardware.Available = hardwareDto.Available;
            existHardware.Deployed = hardwareDto.Deployed;

            await _context.SaveChangesAsync();
            return existHardware;
        }
    } 
}
