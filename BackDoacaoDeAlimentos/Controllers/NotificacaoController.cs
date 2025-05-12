using BackDoacaoDeAlimentos.Interfaces.Servicos;
using FrontDoacaoDeAlimentos.Models;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/Notificacao")]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoService _notificacaoService;

        public NotificacaoController(INotificacaoService notificacaoService)
        {
            _notificacaoService = notificacaoService;
        }

        [HttpGet("obterTodas")]
        public async Task<IActionResult> ObterTodas()
        {
            var notificacoes = await _notificacaoService.ObterTodasNotificacoes();
            if (notificacoes == null || !notificacoes.Any())
                return NotFound();

            return Ok(notificacoes);
        }

        [HttpGet("obterPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var notificacao = await _notificacaoService.ObterNotificacaoPorId(id);
            if (notificacao == null)
                return NotFound();

            return Ok(notificacao);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Notificacao notificacao)
        {
            if (notificacao == null)
                return BadRequest("A notificação não pode ser nula.");

            var novaNotificacao = await _notificacaoService.AdicionarNotificacao(notificacao);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaNotificacao.Id }, novaNotificacao);
        }
       
        [HttpDelete("deletar/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var notificacao = await _notificacaoService.ObterNotificacaoPorId(id);
            if (notificacao == null)
                return NotFound();

            await _notificacaoService.RemoverNotificacao (id);
            return NoContent();
        }
    }
}
