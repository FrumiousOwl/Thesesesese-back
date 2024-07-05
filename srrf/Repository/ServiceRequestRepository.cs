
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Interface;
using srrf.Models;

namespace srrf.Repository
{
    public class ServiceRequestRepository : IServiceRequest
    {
        private readonly SrrfContext _context;
        public ServiceRequestRepository (SrrfContext context)
        {
            _context = context;
        }

        public ServiceRequest GetServiceRequest(int id)
        {
            return _context.ServiceRequests.Where(s => s.Id == id).FirstOrDefault();
        }

        public ServiceRequest GetServiceRequestByName(string name)
        {
            return _context.ServiceRequests.Where(n => n.Name == name).FirstOrDefault();
        }

        public ICollection<ServiceRequest> GetServiceRequests()
        {
            return _context.ServiceRequests.ToList();
        }

        public bool ServiceRequestExists(int SRId)
        {
            return _context.ServiceRequests.Any(s => s.Id == SRId);
        }
    }
}
