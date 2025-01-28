using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.Hardware;
using srrf.Interfaces;
using srrf.Models;
using srrf.Queries;

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

        public async Task<List<Hardware>> GetAllAsync(QueryHardware queryHardware)
        {
            var hardware = _context.Hardware.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryHardware.NameOfHardware))
            {
                hardware = hardware.Where(h => h.Name.Contains(queryHardware.NameOfHardware));
            }

            if (!string.IsNullOrWhiteSpace(queryHardware.Descriptions))
            {
                hardware = hardware.Where(h => h.Description.Contains(queryHardware.Descriptions));
            }

            return await hardware.ToListAsync();
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
