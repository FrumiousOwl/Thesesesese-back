
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Interface;
using srrf.Models;

namespace srrf.Repository
{
    public class ServiceRequestRepository : IServiceRequest
    {
        private readonly SrrfContext _context;
        public ServiceRequestRepository(SrrfContext context)
        {
            _context = context;
        }
        public bool CreateRequest(ServiceRequest serviceRequest)
        {
            _context.Add(serviceRequest);
            return Save();
        }

        public bool DeleteRequest(ServiceRequest serviceRequest)
        {
            _context.Remove(serviceRequest);
            return Save();
        }

        public ServiceRequest GetServiceRequest(int id)
        {
            return _context.ServiceRequests.Where(s => s.Id == id).FirstOrDefault();
        }

        public ICollection<ServiceRequest> GetServiceRequests()
        {
            return _context.ServiceRequests.OrderBy(s => s.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool ServiceRequestExists(int SRId)
        {
            return _context.ServiceRequests.Any(s => s.Id == SRId);
        }

        public bool UpdateRequest(ServiceRequest serviceRequest)
        {
            _context.Update(serviceRequest);
            return Save();
        }
    }
}
