using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.HardwareRequest;
using srrf.Interfaces;
using srrf.Models;
using srrf.Queries;

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

        public async Task<List<HardwareRequest>> GetAllAsync(QueryRequestz query)
        {
            var request = _context.HardwareRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                request = request.Where(r => r.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Department))
            {
                request = request.Where(r => r.Department.Contains(query.Department));
            }

            if (!string.IsNullOrWhiteSpace(query.Workstation))
            {
                request = request.Where(r => r.Workstation.Contains(query.Workstation));
            }

            return await request.ToListAsync();
        }

        public async Task<HardwareRequest?> GetByIdAsync(int id)
        {
            return await _context.HardwareRequests.Include(h => h.Hardware).FirstOrDefaultAsync(x => x.RequestId == id);
        }

        public async Task<HardwareRequest?> UpdateAsync(int id, HardwareRequestCUDDto hardwareRequestDto, string userName, List<string> userRoles)
        {
            var existRequest = await _context.HardwareRequests.FirstOrDefaultAsync(x => x.RequestId == id);

            if (existRequest == null)
            {
                return null;
            }

            bool isAdminOrManager = userRoles.Contains("RequestManager");

            if (!isAdminOrManager && existRequest.Name != userName)
            {
                return null; 
            }

            existRequest.Department = hardwareRequestDto.Department;
            existRequest.Workstation = hardwareRequestDto.Workstation;
            existRequest.Problem = hardwareRequestDto.Problem;
            existRequest.DateNeeded = hardwareRequestDto.DateNeeded;
            existRequest.HardwareId = hardwareRequestDto.HardwareId;

            if (isAdminOrManager)
            {
                existRequest.IsFulfilled = hardwareRequestDto.IsFulfilled;
                existRequest.SerialNo = hardwareRequestDto.SerialNo;
            }

            await _context.SaveChangesAsync();

            return existRequest;
        }

    }

}
