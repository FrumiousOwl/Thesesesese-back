using Microsoft.AspNetCore.Mvc;
using srrf.MachineLearning;

namespace srrf.Controllers
{
    [ApiController]
    [Route("api/anomalydetection")]
    public class AnomalyDetectionController : ControllerBase
    {
        private readonly AnomalyDetector _detector;

        public AnomalyDetectionController(AnomalyDetector detector)
        {
            _detector = detector;
        }

        [HttpGet("detect")]
        public IActionResult DetectAnomalies()
        {
            _detector.TrainAndDetectAnomalies();
            return Ok("Anomaly detection completed.");
        }
    }
}
