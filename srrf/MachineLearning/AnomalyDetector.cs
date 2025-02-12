using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace srrf.MachineLearning
{
    public class AnomalyDetector
    {
        private readonly MLContext _mlContext;
        private readonly IDataLoader _dataLoader;

        public AnomalyDetector(IDataLoader dataLoader)
        {
            _mlContext = new MLContext();
            _dataLoader = dataLoader;
        }

        public void TrainAndDetectAnomalies()
        {
            var auditLogs = _dataLoader.GetAuditLogData();
            var dataView = _mlContext.Data.LoadFromEnumerable(auditLogs);

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding("RoleEncoded", "Role")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("EntityEncoded", "EntityName"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ActionEncoded", "Action"))
                .Append(_mlContext.Transforms.Concatenate("Features", "RoleEncoded", "EntityEncoded", "ActionEncoded"))
                .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(featureColumnName: "Features", rank: 2));

            var model = pipeline.Fit(dataView);
            var transformedData = model.Transform(dataView);
            var predictions = _mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false);

            Console.WriteLine("Anomaly Detection Results:");
            foreach (var (log, pred) in auditLogs.Zip(predictions, (log, pred) => (log, pred)))
            {
                Console.WriteLine($"Email: {log.Email}, Role: {log.Role}, Entity: {log.EntityName}, Action: {log.Action}, Prediction (Anomaly?): {pred.Prediction}");
            }
        }
    }
}
