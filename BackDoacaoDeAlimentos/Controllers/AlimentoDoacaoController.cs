using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/alimentoDoacao")]
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
            try
            {
                var alimentos = await _alimentoDoacaoService.ListarAlimentosPorDoacao(doacaoId);
                if (alimentos == null || !alimentos.Any())
                    return NotFound("Nenhum alimento encontrado para essa doação.");

                return Ok(alimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar alimentos da doação.", details = ex.Message });
            }
        }

        [HttpGet("obterPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var alimento = await _alimentoDoacaoService.ObterPorId(id);
                if (alimento == null)
                    return NotFound("Alimento não encontrado.");

                return Ok(alimento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar alimento.", details = ex.Message });
            }
        }

        [HttpGet("proximosVencimentos")]
        public async Task<IActionResult> ObterProximosVencimentos([FromQuery] int diasAntecedencia = 7)
        {
            try
            {
                var alimentos = await _alimentoDoacaoService.ListarAlimentosProximosVencimento(diasAntecedencia);
                return Ok(alimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar alimentos com vencimento próximo.", details = ex.Message });
            }
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] AlimentoDoacao alimentoDoacao)
        {
            try
            {
                if (alimentoDoacao == null)
                    return BadRequest("O alimento da doação não pode ser nulo.");

                var novoAlimento = await _alimentoDoacaoService.AdicionarAlimentoADoacao(alimentoDoacao);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoAlimento.Id }, novoAlimento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao adicionar alimento à doação.", details = ex.Message });
            }
        }

        [HttpPut("atualizarQuantidade/{id}")]
        public async Task<IActionResult> AtualizarQuantidade(int id, [FromBody] decimal novaQuantidade)
        {
            try
            {
                await _alimentoDoacaoService.AtualizarQuantidadeAlimento(id, novaQuantidade);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao atualizar quantidade do alimento.", details = ex.Message });
            }
        }

        [HttpDelete("deletar/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var alimento = await _alimentoDoacaoService.ObterPorId(id);
                if (alimento == null)
                    return NotFound("Alimento não encontrado.");

                await _alimentoDoacaoService.Remover(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao deletar alimento da doação.", details = ex.Message });
            }
        }
    }
}
