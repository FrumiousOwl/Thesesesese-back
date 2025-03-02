using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using srrf.Data;
using srrf.Models;

namespace srrf.MachineLearning
{
    public class AnomalyDetector
    {
        private readonly MLContext _mlContext;
        private readonly IServiceProvider _serviceProvider;

        private ITransformer _model;

        public AnomalyDetector(IServiceProvider serviceProvider)
        {
            _mlContext = new MLContext();
            _serviceProvider = serviceProvider;
            TrainModel(); 
        }

        private void TrainModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataLoader = scope.ServiceProvider.GetRequiredService<IDataLoader>();
                var dataView = _mlContext.Data.LoadFromEnumerable(dataLoader.GetAuditLogData());

                var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding("RoleEncoded", "Role")
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("EntityEncoded", "EntityName"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ActionEncoded", "Action"))
                    .Append(_mlContext.Transforms.Concatenate("Features", "RoleEncoded", "EntityEncoded", "ActionEncoded"))
                    .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(featureColumnName: "Features", rank: 4));

                _model = pipeline.Fit(dataView);
            }
        }

        public bool Predict(AuditLogModel log)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataLoader = scope.ServiceProvider.GetRequiredService<IDataLoader>();
                var data = new List<AuditLogModel> { log };
                var dataView = _mlContext.Data.LoadFromEnumerable(data);

                var transformedData = _model.Transform(dataView);
                var predictions = _mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false).ToList();

                return predictions.FirstOrDefault()?.Prediction ?? false;
            }
        }

        /*public async Task TrainAndDetectAnomaliesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var anomalyLogService = scope.ServiceProvider.GetRequiredService<AnomalyLogService>();

                var auditLogs = await dbContext.AuditLogs
                    .Where(log => !log.IsAnalyzed)
                    .ToListAsync();

                foreach (var log in auditLogs)
                {
                    var auditLogModel = new AuditLogModel
                    {
                        Email = log.Email,
                        Role = log.Role,
                        EntityName = log.EntityName,
                        Action = log.Action
                    };

                    bool isAnomalous = Predict(auditLogModel);

                    await anomalyLogService.SaveAnomalyLogAsync(auditLogModel, isAnomalous);

                    log.IsAnalyzed = true;
                    await dbContext.SaveChangesAsync();
                }
            }
        }*/


/*        public async Task<bool> PredictAndSaveAsync(AuditLogModel log)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var anomalyLogService = scope.ServiceProvider.GetRequiredService<AnomalyLogService>();

                var data = new List<AuditLogModel> { log };
                var dataView = _mlContext.Data.LoadFromEnumerable(data);
                var transformedData = _model.Transform(dataView);
                var prediction = _mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false).FirstOrDefault()?.Prediction ?? false;

                await anomalyLogService.SaveAnomalyLogAsync(log, prediction);
                return prediction;
            }
        }*/
    }

}
