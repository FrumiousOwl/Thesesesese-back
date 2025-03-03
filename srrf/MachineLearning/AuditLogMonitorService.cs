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
                        .Where(log => !log.IsAnalyzed) 
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
                        bool isAnomalous = _anomalyDetector.Predict(auditLogModel);
                        Console.WriteLine($"Email: {log.Email}, Role: {log.Role}, Entity: {log.EntityName}, Action: {log.Action}, Anomalous: {isAnomalous}");

                        await SaveAnomalyLogAsync(dbContext, log, isAnomalous);

                        log.IsAnalyzed = true;
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(6666, stoppingToken); 
            }
        }

        private async Task SaveAnomalyLogAsync(Context dbContext, AuditLog log, bool isAnomalous)
        {
            var anomalyLog = new AnomalyLog
            {
                Email = log.Email,
                Role = log.Role,
                Entity = log.EntityName,
                Action = log.Action,
                IsAnomaly = isAnomalous,
                Timestamp = log.TimeStamp
            };

            dbContext.AnomalyLogs.Add(anomalyLog);
            await dbContext.SaveChangesAsync();
        }


    }
}
