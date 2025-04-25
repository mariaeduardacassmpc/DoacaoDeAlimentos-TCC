using TCCDoacaoDeAlimentos.Shared.Models;   
using Microsoft.AspNetCore.Mvc;
using BackDoacaoDeAlimentos.Interfaces.Servicos;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/alimento")]
    public class AlimentosController : ControllerBase
    {
        // Campo privado e readonly para garantir que a dependência seja atribuída no construtor e não possa ser alterada depois
        private readonly IAlimentoService _alimentoService;

        public AlimentosController(IAlimentoService alimentoService)
        {
            _alimentoService = alimentoService;
        }

        [HttpGet("obterAlimentos")]
        public IActionResult ObterAlimentos()
        {
            var alimento = _alimentoService.ObterTodosAlimentos();
            if (alimento == null)
                return NotFound();

            return Ok(alimento);
        }

        [HttpGet("obterAlimento/{id}")]
        public IActionResult ObterAlimento(int id)
        {
            var alimento = _alimentoService.ObterAlimentoPorId(id);
            if (alimento == null)
                return NotFound();

            return Ok(alimento);
        }

        [HttpPost("gravar")]
        public IActionResult GravarAlimento(Alimento alimento)
        {
            _alimentoService.AdicionarAlimento(alimento);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult RemoverAlimento(int id)
        {
            var alimento = _alimentoService.ObterAlimentoPorId(id);
            if (alimento == null)
                return NotFound();

            _alimentoService.RemoverAlimento(id);
            return NoContent();
        }
    } 
}
