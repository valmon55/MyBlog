using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Глобальный обработчик ошибок
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        [HttpGet]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                _logger.LogError($"Произошла ошибка с кодом: {statusCode}");

                if (statusCode == 404)
                {
                    return StatusCode(404);
                }
                else if (statusCode == 403)
                {
                    return StatusCode(403);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                _logger.LogInformation($"Произошла ошибка, код ошибки неизвестен...");
                return StatusCode(500);
            }
        }
    }
}
