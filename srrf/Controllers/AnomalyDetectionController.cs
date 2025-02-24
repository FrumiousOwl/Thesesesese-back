using Microsoft.AspNetCore.Mvc;
using srrf.Data;
using srrf.MachineLearning;

namespace srrf.Controllers
{
    [Route("api/anomalyDetector")]
    [ApiController]

    public class AnomalyDetectionController : ControllerBase
    {
        private readonly AnomalyDetector _detector;
        private readonly Context _context;

        public AnomalyDetectionController(AnomalyDetector detector, Context context)
        {
            _detector = detector;
            _context = context;
        }

        [HttpPost("detectAnomalies")]
        public IActionResult DetectAnomalies()
        {
            _detector.TrainAndDetectAnomalies();
            return Ok("Anomaly detection completed.");
        }
    }
}
