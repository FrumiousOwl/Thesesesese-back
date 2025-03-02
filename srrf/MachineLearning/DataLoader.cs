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


            // system manager permissions
            logs.Add(new AuditLogModel { Email = "systemManager@example.com", Role = "SystemManager", EntityName = "User", Action = "Added" });
            logs.Add(new AuditLogModel { Email = "systemManager@example.com", Role = "SystemManager", EntityName = "User", Action = "Modified" });
            logs.Add(new AuditLogModel { Email = "systemManager@example.com", Role = "SystemManager", EntityName = "User", Action = "Deleted" });

            logs.Add(new AuditLogModel { Email = "systemManager@example.com", Role = "SystemManager", EntityName = "IdentityUserRole`1", Action = "Added" });
            logs.Add(new AuditLogModel { Email = "systemManager@example.com", Role = "SystemManager", EntityName = "IdentityUserRole`1", Action = "Modified" });
            logs.Add(new AuditLogModel { Email = "systemManager@example.com", Role = "SystemManager", EntityName = "IdentityUserRole`1", Action = "Deleted" });

            // Inventory manager permissions
            logs.Add(new AuditLogModel { Email = "dzfddda@gmail.com", Role = "InventoryManager", EntityName = "Hardware", Action = "Added" });
            logs.Add(new AuditLogModel { Email = "inventoryManager@example.com", Role = "InventoryManager", EntityName = "Hardware", Action = "Modified" });
            logs.Add(new AuditLogModel { Email = "vSDvSegxc@example.com", Role = "InventoryManager", EntityName = "Hardware", Action = "Deleted" });
            logs.Add(new AuditLogModel { Email = "Lulquidiatez3@gmail.com", Role = "InventoryManager", EntityName = "Hardware", Action = "Deleted" });
            logs.Add(new AuditLogModel { Email = "vzr55hbh@gmail.com", Role = "InventoryManager", EntityName = "Hardware", Action = "Deleted" });
            logs.Add(new AuditLogModel { Email = "Billyjeanyzs@example.com", Role = "InventoryManager", EntityName = "Hardware", Action = "Deleted" });

            // Request Manager permissions
            logs.Add(new AuditLogModel { Email = "Abdgsdgj@gmail.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Added" });
            logs.Add(new AuditLogModel { Email = "requestManager@example.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Modified" });
            logs.Add(new AuditLogModel { Email = "dvfkmg54i63gmail.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Added" });
            logs.Add(new AuditLogModel { Email = "requestManager@example.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Modified" });
            logs.Add(new AuditLogModel { Email = "fb7zdfcbxcfv@example.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Modified" });
            logs.Add(new AuditLogModel { Email = "fvdfcn6y5@gmail.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Added" });
            logs.Add(new AuditLogModel { Email = "FallClown209@gmail.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Deleted" });
            logs.Add(new AuditLogModel { Email = "Turdra22@gmail.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Deleted" });
            logs.Add(new AuditLogModel { Email = "requestManager@example.com", Role = "RequestManager", EntityName = "HardwareRequest", Action = "Deleted" });

            // User permissions
            logs.Add(new AuditLogModel { Email = "user@example.com", Role = "User", EntityName = "HardwareRequest", Action = "Added" });

            return logs;
        }

    }
}
