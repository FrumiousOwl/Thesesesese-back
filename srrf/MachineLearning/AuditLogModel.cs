using Microsoft.ML.Data;

namespace srrf.MachineLearning
{
    public class AuditLogModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
    }

    public class AnomalyPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
    }
}
