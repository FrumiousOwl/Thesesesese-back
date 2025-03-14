using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.Hardware;
using srrf.Interfaces;
using srrf.Models;
using srrf.Queries;
using srrf.Service;

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
            hardwareModel.Supplier = AesEncryption.Encrypt(hardwareModel.Supplier);
            hardwareModel.TotalPrice = AesEncryption.Encrypt(hardwareModel.TotalPrice);

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

        public async Task<IEnumerable<AvailableHardwareDto>> GetAvailableHardwareAsync(QueryAvailableHardware query, bool isInventoryManager)
        {
            var availableHardware = _context.Hardware
                .Where(h => h.Available > 0);

            if (!string.IsNullOrEmpty(query.Descriptions))
            {
                availableHardware = availableHardware.Where(h => h.Description.Contains(query.Descriptions));
            }
            if (!string.IsNullOrEmpty(query.NameOfHardware))
            {
                availableHardware = availableHardware.Where(h => h.Name.Contains(query.NameOfHardware));
            }

            return await availableHardware.Select(h => new AvailableHardwareDto
            {
                HardwareId = h.HardwareId,
                Name = h.Name,
                Available = h.Available,
                Deployed = h.Deployed,
                DatePurchased = h.DatePurchased,
                Supplier = h.Supplier, 
                IsInventoryManager = isInventoryManager
            }).ToListAsync();
        }


        public async Task<Hardware?> GetByIdAsync(int id)
        {
            return await _context.Hardware.FindAsync(id);
        }

        public async Task<IEnumerable<DefectiveHardwareDto>> GetDefectiveHardwareAsync(QueryDefectiveHardware query, bool isInventoryManager)
        {
            var defectiveHardware = _context.Hardware
                .Where(h => h.Defective > 0);

            if (!string.IsNullOrEmpty(query.Descriptions))
            {
                defectiveHardware = defectiveHardware.Where(h => h.Description.Contains(query.Descriptions));
            }
            if (!string.IsNullOrEmpty(query.NameOfHardware))
            {
                defectiveHardware = defectiveHardware.Where(h => h.Name.Contains(query.NameOfHardware));
            }

            return await defectiveHardware.Select(h => new DefectiveHardwareDto
            {
                HardwareId = h.HardwareId,
                Name = h.Name,
                Defective = h.Defective,
                DatePurchased = h.DatePurchased,
                Supplier = h.Supplier,
                IsInventoryManager = isInventoryManager
            }).ToListAsync();
        }

        public async Task<Hardware?> UpdateAsync(int id, HardwareCUDDto hardwareDto)
        {
            var existHardware = await _context.Hardware.FirstOrDefaultAsync(x => x.HardwareId == id);

            if (existHardware == null)
            {
                return null;
            }

            existHardware.Name = hardwareDto.Name;
            existHardware.Description = hardwareDto.Description;
            existHardware.DatePurchased = hardwareDto.DatePurchased;
            existHardware.Supplier = AesEncryption.Encrypt(hardwareDto.Supplier);
            existHardware.Defective = hardwareDto.Defective;
            existHardware.Available = hardwareDto.Available;
            existHardware.Deployed = hardwareDto.Deployed;
            existHardware.TotalPrice = AesEncryption.Encrypt(hardwareDto.TotalPrice);

            await _context.SaveChangesAsync();
            return existHardware;
        }

    }
}
