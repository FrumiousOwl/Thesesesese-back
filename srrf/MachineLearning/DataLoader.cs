using srrf.Data;

namespace srrf.MachineLearning
{
    public class DataLoader : IDataLoader
    {
        private readonly Context _context;

        public DataLoader(Context context)
        {
            _context = context;
        }

        public List<AuditLogModel> GetAuditLogData()
        {
            return _context.AuditLogs
                .Where(a => a.EntityName == "User" || a.EntityName == "Request" || a.EntityName == "Hardware")
                .Select(a => new AuditLogModel
                {
                    Email = a.Email,
                    Role = a.Role,
                    EntityName = a.EntityName,
                    Action = a.Action
                })
                .ToList();
        }
    }
}
