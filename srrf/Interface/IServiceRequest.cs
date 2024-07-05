using srrf.Models;

namespace srrf.Interface
{
    public interface IServiceRequest
    {
        ICollection<ServiceRequest> GetServiceRequests();
        ServiceRequest GetServiceRequest(int id);
        ServiceRequest GetServiceRequestByName(string name);
        bool ServiceRequestExists(int SRId);
    }
}
