using Microsoft.AspNetCore.Mvc;
using Sessions_app.Service;

namespace Sessions_app.Controllers
{
    [ApiController]
    [Route("api/modelo")]
    public class ModeloController : ControllerBase
    {
        private readonly RiskPredictionService _predictionService;

        public ModeloController(RiskPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost("retreinar")]
        public IActionResult RetreinarModelo()
        {
            _predictionService.TrainModel();
            return Ok(new { message = "Modelo retreinado com sucesso!" });
        }
    }
}
