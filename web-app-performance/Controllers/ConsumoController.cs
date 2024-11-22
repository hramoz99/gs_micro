using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using web_app_domain;
using web_app_repository;

namespace web_app_performance.Controllers
{
    [Route("/consumo")]
    [ApiController]
    public class ConsumoController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        private readonly IConsumoRepository _repository;

        public ConsumoController(IConsumoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumo()
        {
            var consumos = await _repository.ListarConsumos();
            if(consumos == null)
                return NotFound();

            string consumosJson = JsonConvert.SerializeObject(consumos);
            return Ok(consumos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Consumo consumo)
        {
            await _repository.SalvarConsumo(consumo);         

            return Ok(new {mensagem = "Criado com sucesso!"});
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Consumo consumo)
        {
            await _repository.AtualizarConsumo(consumo);

            string key = "getconsumo";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoverConsumo(id);

            //apagar o cachê
            string key = "getconsumo";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();
        }
    }
}
