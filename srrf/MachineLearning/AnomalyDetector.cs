using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
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
                var auditLogs = dataLoader.GetAuditLogData();
                var dataView = _mlContext.Data.LoadFromEnumerable(auditLogs);

                var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding("RoleEncoded", "Role")
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("EntityEncoded", "EntityName"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ActionEncoded", "Action"))
                    .Append(_mlContext.Transforms.Concatenate("Features", "RoleEncoded", "EntityEncoded", "ActionEncoded"))
                    .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(featureColumnName: "Features", rank: 4));

                _model = pipeline.Fit(dataView);
            }
        }

        public void TrainAndDetectAnomalies()
        {
            using (var scope = _serviceProvider.CreateScope()) 
            {
                var dataLoader = scope.ServiceProvider.GetRequiredService<IDataLoader>(); 
                var auditLogs = dataLoader.GetAuditLogData();
                var dataView = _mlContext.Data.LoadFromEnumerable(auditLogs);

                var transformedData = _model.Transform(dataView);
                var predictions = _mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false);

                Console.WriteLine("Anomaly Detection Results:");
                foreach (var (log, pred) in auditLogs.Zip(predictions, (log, pred) => (log, pred)))
                {
                    Console.WriteLine($"Email: {log.Email}, Role: {log.Role}, Entity: {log.EntityName}, Action: {log.Action}, Prediction (Anomaly?): {pred.Prediction}");
                }
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
    }

}
