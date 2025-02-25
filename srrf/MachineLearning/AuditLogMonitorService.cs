using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using srrf.Data;
using srrf.Models;

namespace srrf.MachineLearning
{
    public class AuditLogMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AnomalyDetector _anomalyDetector;
        private int _lastProcessedId = 0;

        public AuditLogMonitorService(IServiceScopeFactory scopeFactory, AnomalyDetector anomalyDetector)
        {
            _scopeFactory = scopeFactory;
            _anomalyDetector = anomalyDetector;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                    var latestLogs = await dbContext.AuditLogs
                        .Where(log => log.Id > _lastProcessedId)
                        .OrderBy(log => log.Id)
                        .ToListAsync(stoppingToken);

                    foreach (var log in latestLogs)
                    {

                        var auditLogModel = new AuditLogModel
                        {
                            Email = log.Email,
                            Role = log.Role,
                            EntityName = log.EntityName,
                            Action = log.Action
                        };

/*                        if (IsAllowedAction(auditLogModel))
                        {
                            Console.WriteLine($"✅ Allowed action: {log.Role} {log.Action} on {log.EntityName} (Skipping anomaly check)");
                            return; 
                        }*/

                        bool isAnomalous = _anomalyDetector.Predict(auditLogModel);
                        Console.WriteLine($"Email: {log.Email}, Role: {log.Role}, Entity: {log.EntityName}, Action: {log.Action}, Anomalous: {isAnomalous}");

                        _lastProcessedId = log.Id; 
                    }
                }

                await Task.Delay(20000, stoppingToken); // Check every 20 seconds
            }
        }

/*        private bool IsAllowedAction(AuditLogModel log)
        {
            return (log.Role == "RequestManager" && log.EntityName == "Request" && log.Action == "Deleted") ||
                   (log.Role == "InventoryManager" && log.EntityName == "Hardware" && log.Action == "Deleted");
        }*/

    }
}
