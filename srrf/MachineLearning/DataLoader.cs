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
            var logs = _context.AuditLogs
                .Select(a => new AuditLogModel
                {
                    Email = a.Email,
                    Role = a.Role,
                    EntityName = a.EntityName,
                    Action = a.Action
                })
                .ToList();

            // Ensure training data includes valid SystemManager actions
            logs.Add(new AuditLogModel { Email = "admin@example.com", Role = "SystemManager", EntityName = "User", Action = "Modify" });
            logs.Add(new AuditLogModel { Email = "admin@example.com", Role = "SystemManager", EntityName = "User", Action = "Delete" });
            logs.Add(new AuditLogModel { Email = "admin@example.com", Role = "SystemManager", EntityName = "IdentityUserRole`1", Action = "Modify" });
            logs.Add(new AuditLogModel { Email = "admin@example.com", Role = "SystemManager", EntityName = "IdentityUserRole`1", Action = "Delete" });

            return logs;
        }

    }
}
