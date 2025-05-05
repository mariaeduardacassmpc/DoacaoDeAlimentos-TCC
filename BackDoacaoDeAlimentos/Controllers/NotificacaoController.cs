using Microsoft.AspNetCore.Mvc;


namespace BackDoacaoDeAlimentos.Controllers
{
    [Route("api/Notificacao")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        [HttpGet("obterTodas")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("obter/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("adicionar")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
