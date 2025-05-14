using BackDoacaoDeAlimentos.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/alimento-doacao")]
    public class AlimentoDoacaoController : ControllerBase
    {
        private readonly IAlimentoDoacaoService _alimentoDoacaoService;

        public AlimentoDoacaoController(IAlimentoDoacaoService alimentoDoacaoService)
        {
            _alimentoDoacaoService = alimentoDoacaoService;
        }

        [HttpGet("obterTodosPorDoacao/{doacaoId}")]
        public async Task<IActionResult> ObterTodosPorDoacao(int doacaoId)
        {
            var alimentos = await _alimentoDoacaoService.ListarAlimentosPorDoacaoAsync(doacaoId);
            if (alimentos == null || !alimentos.Any())
                return NotFound("Nenhum alimento encontrado para essa doação.");

            return Ok(alimentos);
        }

        [HttpGet("obterPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var alimento = await _alimentoDoacaoService.ObterPorIdAsync(id);
            if (alimento == null)
                return NotFound("Alimento não encontrado.");

            return Ok(alimento);
        }

        [HttpGet("proximosVencimentos")]
        public async Task<IActionResult> ObterProximosVencimentos([FromQuery] int diasAntecedencia = 7)
        {
            var alimentos = await _alimentoDoacaoService.ListarAlimentosProximosVencimentoAsync(diasAntecedencia);
            return Ok(alimentos);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] AlimentoDoacao alimentoDoacao)
        {
            if (alimentoDoacao == null)
                return BadRequest("O alimento da doação não pode ser nulo.");

            var novoAlimento = await _alimentoDoacaoService.AdicionarAlimentoADoacaoAsync(alimentoDoacao);
            return CreatedAtAction(nameof(ObterPorId), new { id = novoAlimento.Id }, novoAlimento);
        }

        [HttpPut("atualizarQuantidade/{id}")]
        public async Task<IActionResult> AtualizarQuantidade(int id, [FromBody] decimal novaQuantidade)
        {
            try
            {
                await _alimentoDoacaoService.AtualizarQuantidadeAlimentoAsync(id, novaQuantidade);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deletar/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var alimento = await _alimentoDoacaoService.ObterPorIdAsync(id);
            if (alimento == null)
                return NotFound("Alimento não encontrado.");

            await _alimentoDoacaoService.RemoverAlimentoDeDoacaoAsync(id);
            return NoContent();
        }
    }
}
