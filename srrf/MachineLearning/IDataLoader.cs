using System.Collections.Generic;

namespace srrf.MachineLearning
{
    public interface IDataLoader
    {
        List<AuditLogModel> GetAuditLogData();
    }
}
