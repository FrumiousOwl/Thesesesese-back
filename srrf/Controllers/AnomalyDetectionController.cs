using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using srrf.Data;
using srrf.MachineLearning;

namespace srrf.Controllers
{
    [Authorize(Roles = "SystemManager")]
    [Route("api/anomalyDetector")]
    [ApiController]

    public class AnomalyDetectionController : ControllerBase
    {
        private readonly AnomalyDetector _detector;
        private readonly Context _context;
        private readonly AnomalyLogService _anomalyLogService;

        public AnomalyDetectionController(AnomalyDetector detector, Context context, AnomalyLogService anomalyLogService)
        {
            _detector = detector;
            _context = context;
            _anomalyLogService = anomalyLogService;
        }

/*        [HttpPost("detectAnomalies")]

        public IActionResult DetectAnomalies()
        {
            _detector.TrainAndDetectAnomaliesAsync();
            return Ok("Anomaly detection completed.");
        }*/

        [HttpGet("logs")]
        public async Task<IActionResult> GetAllAnomalyLogs()
        {
            var logs = await _anomalyLogService.GetAllAnomaliesAsync();
            return Ok(logs);
        }
    }
}
