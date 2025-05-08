using TCCDoacaoDeAlimentos.Shared.Models;   
using Microsoft.AspNetCore.Mvc;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Servicos;
using TCCDoacaoDeAlimentos.Models;

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

        [HttpDelete("deletar/{id}")]
        public ActionResult RemoverAlimento(int id)
        {
            var alimento = _alimentoService.ObterAlimentoPorId(id);
            if (alimento == null)
                return NotFound();

            _alimentoService.RemoverAlimento(id);
            return NoContent();
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Alimento alimento)
        {
            if (id != alimento.Id)
                return BadRequest("O ID da URL não bate com o ID do corpo.");

            var ongExistente = await _alimentoService.ObterAlimentoPorId(id);
            if (ongExistente == null)
                return NotFound();

            await _alimentoService.AtualizarAlimento(alimento);
            return NoContent();
        }
    } 
}
