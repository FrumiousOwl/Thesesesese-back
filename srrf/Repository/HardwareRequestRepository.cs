using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.HardwareRequest;
using srrf.Interfaces;
using srrf.Models;

namespace srrf.Repository
{
    public class HardwareRequestRepository : IHardwareRequestRepository
    {
        private readonly Context _context;
        public HardwareRequestRepository(Context context)
        {
            _context = context;    
        }
        public async Task<HardwareRequest> CreateAsync(HardwareRequest hardwareRequestModel)
        {
            await _context.HardwareRequests.AddAsync(hardwareRequestModel);
            await _context.SaveChangesAsync();
            return hardwareRequestModel;
        }

        public async Task<HardwareRequest?> DeleteAsync(int id)
        {
            var hardwareRequestToDelete = await _context.HardwareRequests.FirstOrDefaultAsync(x => x.RequestId == id);

            if (hardwareRequestToDelete == null)
            {
                return null;
            }

            _context.HardwareRequests.Remove(hardwareRequestToDelete);
            await _context.SaveChangesAsync();
            return hardwareRequestToDelete;
        }

        public async Task<List<HardwareRequest>> GetAllAsync()
        {
            return await _context.HardwareRequests.Include(h => h.Hardware).Include(u => u.User).ToListAsync();
        }

        public async Task<HardwareRequest?> GetByIdAsync(int id)
        {
            return await _context.HardwareRequests.Include(h => h.Hardware).Include(u => u.User).FirstOrDefaultAsync(x => x.RequestId == id);
        }

        public async Task<HardwareRequest?> UpdateAsync(int id, HardwareRequestCUDDto hardwareRequestDto)
        {
            var existRequest = await _context.HardwareRequests.FirstOrDefaultAsync(x => x.RequestId == id);

            if (existRequest == null)
            {
                return null;
            }

            existRequest.Name = hardwareRequestDto.Name;
            existRequest.Department = hardwareRequestDto.Department;
            existRequest.Workstation = hardwareRequestDto.Workstation;
            existRequest.Problem = hardwareRequestDto.Problem;
            existRequest.IsFulfilled = hardwareRequestDto.IsFulfilled;
            existRequest.DateNeeded = hardwareRequestDto.DateNeeded;
            existRequest.HardwareId = hardwareRequestDto.HardwareId;
            existRequest.UserId = hardwareRequestDto.UserId;

            await _context.SaveChangesAsync();

            return existRequest;
        }
    }

}
