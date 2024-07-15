using srrf.Models;

namespace srrf.Interface
{
    public interface IServiceRequest
    {
        ICollection<ServiceRequest> GetServiceRequests();
        ServiceRequest GetServiceRequest(int id);
        bool ServiceRequestExists(int SRId);
        bool CreateRequest(ServiceRequest serviceRequest);
        bool UpdateRequest(ServiceRequest serviceRequest);
        bool DeleteRequest(ServiceRequest serviceRequest);
        bool Save();
    }
}
