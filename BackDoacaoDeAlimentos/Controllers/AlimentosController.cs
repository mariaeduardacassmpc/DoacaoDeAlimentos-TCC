using TCCDoacaoDeAlimentos.Shared.Models;   
using Microsoft.AspNetCore.Mvc;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Servicos;

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
        public async Task<IActionResult> ObterTodos()
        {
            var alimento = await _alimentoService.ObterTodosAlimentos();
            if (alimento == null)
                return NotFound();

            return Ok(alimento);
        }

        [HttpGet("obterAlimento/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var alimento = await _alimentoService.ObterAlimentoPorId(id);
            if (alimento == null)
                return NotFound();

            return Ok(alimento);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Alimento alimento)
        {
            try
            {
                await _alimentoService.AdicionarAlimento(alimento);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deletar/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var alimento = await _alimentoService.ObterAlimentoPorId(id);
            if (alimento == null)
                return NotFound();

            await _alimentoService.DeletarAlimento(id);
            return NoContent();
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Alimento alimento)
        {
            var alimentoExistente = await _alimentoService.ObterAlimentoPorId(alimento.Id);
            if (alimentoExistente == null)
                return NotFound();

            await _alimentoService.AtualizarAlimento(alimento);
            return NoContent();
        }
    } 
}
