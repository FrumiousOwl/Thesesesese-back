using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using srrf.Data;
using srrf.Models;

namespace srrf.MachineLearning
{
    public class AuditLogMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AnomalyDetector _anomalyDetector;
        private int _lastProcessedId = 0;

        public AuditLogMonitorService(IServiceScopeFactory serviceScopeFactory, AnomalyDetector anomalyDetector)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _anomalyDetector = anomalyDetector;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<Context>();
                
                    
                    var newLogs = await context.AuditLogs
                        .Where(log =>  log.Id > _lastProcessedId)
                        .OrderBy(log => log.Id)
                        .ToListAsync();

                    if (newLogs.Any())
                    {
                        foreach (var log in newLogs)
                        {
                            var auditLogModel = new AuditLogModel
                            {
                                Email = log.Email,
                                Role = log.Role,
                                EntityName = log.EntityName,
                                Action = log.Action
                            };

                            var prediction = _anomalyDetector.Predict(auditLogModel);


                            Console.WriteLine($"Email: {log.Email}, Role: {log.Role}, Entity: {log.EntityName}, Action: {log.Action}, Prediction (Anomaly?): {prediction.Prediction}");

                            //test
                            /*if (prediction.Prediction)
                            {
                                var anomaly = new AnomalyLog
                                {
                                    Email = log.Email,
                                    Role = log.Role,
                                    EntityName = log.EntityName,
                                    Action = log.Action,
                                    Timestamp = DateTime.Now
                                };

                                dbContext.AnomalyLogs.Add(anomaly);
                                await dbContext.SaveChangesAsync();
                            }*/
                        }

                        _lastProcessedId = newLogs.Max(log => log.Id);
                    }
                }

                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
