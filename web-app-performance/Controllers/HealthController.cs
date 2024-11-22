using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using web_app_domain;
using web_app_repository;

namespace web_app_performance.Controllers
{
    [Route("/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        private readonly IConsumoRepository _repository;

        public HealthController(IConsumoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHealth()
        {
            return Ok("Serviço executando corretamente!");
        }
    }
}
