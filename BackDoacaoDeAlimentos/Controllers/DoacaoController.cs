using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/doacao")]
    public class DoacaoController : ControllerBase
    {
        private readonly IDoacaoService _doacaoService;

        public DoacaoController(IDoacaoService doacaoService)
        {
            _doacaoService = doacaoService;
        }

        [HttpGet("obterTodas")]
        public async Task<IActionResult> ObterTodas()
        {
            var doacoes = await _doacaoService.ObterTodasDoacoes();
            if (doacoes == null)
                return NotFound();

            return Ok(doacoes);
        }

        [HttpGet("obterTodasPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var doacao = await _doacaoService.ObterDoacaoPorId(id);
            if (doacao == null)
                return NotFound();

            return Ok(doacao);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Doacao doacao)
        {
            await _doacaoService.AdicionarDoacao(doacao);
            return CreatedAtAction(nameof(Adicionar), new { id = doacao.Id }, doacao);
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Doacao doacao)
        {
            if (id != doacao.Id)
                return BadRequest("O ID da URL não bate com o ID do corpo.");

            var doacaoExistente = await _doacaoService.ObterDoacaoPorId(id);
            if (doacaoExistente == null)
                return NotFound();

            await _doacaoService.AtualizarDoacao(doacao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var doacao = await _doacaoService.ObterDoacaoPorId(id);
            if (doacao == null)
                return NotFound();

            await _doacaoService.DeletarDoacao(id);
            return NoContent();
        }
    }
}
