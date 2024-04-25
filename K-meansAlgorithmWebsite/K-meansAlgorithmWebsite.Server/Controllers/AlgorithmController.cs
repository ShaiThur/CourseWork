using K_meansAlgorithmWebsite.Server.Models;
using K_meansAlgorithmWebsite.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace K_meansAlgorithmWebsite.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlgorithmController : ControllerBase
    {
        private readonly ClusterService _clusterService;
        private readonly RegressionService _regressionService;

        public AlgorithmController(ClusterService clusterService, RegressionService regressionService)
        {
            _clusterService = clusterService;
            _regressionService = regressionService;
        }

        [HttpPost]
        [Route("ClusterData")]
        public ActionResult<DataResponse> CreateTask([FromBody] DataRequest request)
        {
            _clusterService.RawData = request.RawData;

            if (request.ClustersCount == 0 || request.ClustersCount > request.RawData.Length)
                return BadRequest($"clusters count error {request.ClustersCount}");

            var clusteringResult = _clusterService.Cluster(request.ClustersCount, 100);
            Response.ContentType = "application/json";
            return Ok(new DataResponse { ClusteringResult = clusteringResult });
        }

        [HttpPost]
        [Route("ReceiveCoefficients")]
        public IActionResult GetCoefficients([FromBody] DataRequest request)
        {
            if (request.RawData.GetLength(0) <= 1)
                return BadRequest();
        
            var result = _regressionService.FindCoefficients(request.RawData);

            if (result.Where(r => r.Equals(double.NaN)).Count() > 0)
                return BadRequest();

            return Ok(new RegressionResponse { Coefficients = result });
        }
    }
}
