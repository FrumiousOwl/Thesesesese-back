using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using srrf.Models; // Ensure this matches your project namespace

namespace srrf.MachineLearning
{
    public class AnomalyDetector
    {
        private readonly MLContext _mlContext;
        private readonly IDataLoader _dataLoader;
        private ITransformer _model;

        public AnomalyDetector(IDataLoader dataLoader)
        {
            _mlContext = new MLContext();
            _dataLoader = dataLoader;
            TrainModel(); // Train the model on initialization
        }

        private void TrainModel()
        {
            var auditLogs = _dataLoader.GetAuditLogData();
            var dataView = _mlContext.Data.LoadFromEnumerable(auditLogs);

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding("RoleEncoded", "Role")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("EntityEncoded", "EntityName"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ActionEncoded", "Action"))
                .Append(_mlContext.Transforms.Concatenate("Features", "RoleEncoded", "EntityEncoded", "ActionEncoded"))
                .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(featureColumnName: "Features", rank: 2));

            _model = pipeline.Fit(dataView);
        }

        public void TrainAndDetectAnomalies()
        {
            var auditLogs = _dataLoader.GetAuditLogData();
            var dataView = _mlContext.Data.LoadFromEnumerable(auditLogs);

            var transformedData = _model.Transform(dataView);
            var predictions = _mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false);

            Console.WriteLine("Anomaly Detection Results:");
            foreach (var (log, pred) in auditLogs.Zip(predictions, (log, pred) => (log, pred)))
            {
                Console.WriteLine($"Email: {log.Email}, Role: {log.Role}, Entity: {log.EntityName}, Action: {log.Action}, Prediction (Anomaly?): {pred.Prediction}");
            }
        }

        public AnomalyPrediction Predict(AuditLogModel log)
        {
            var data = new List<AuditLogModel> { log };
            var dataView = _mlContext.Data.LoadFromEnumerable(data);

            var transformedData = _model.Transform(dataView);
            var predictions = _mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false).ToList();

            return predictions.FirstOrDefault();
        }
    }
}
