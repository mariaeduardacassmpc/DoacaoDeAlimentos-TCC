using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Dto;
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

        [HttpGet("obterEstatisticas")]
        public async Task<IActionResult> obterEstatisticas()
        {
            var doacoes = await _doacaoService.ObterEstatisticas();
            if (doacoes == null)
                return Ok(new EstatisticasDto { TotalOngsAjudadas = 0, TotalKgAlimentosDoado = 0 });

            return Ok(doacoes);
        }

        [HttpPost("filtrarDoacao")]
        public async Task<IActionResult> ObterTodasPorEntidade([FromBody] FiltroDoacaoDto filtroDoacaoDto)
        {
            var doacoes = await _doacaoService.ObterDoacoesComFiltro(filtroDoacaoDto);
            if (doacoes == null)
                return NotFound();

            return Ok(doacoes);
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
            return CreatedAtAction(nameof(Adicionar), new { id = doacao.IdDoacao }, doacao);
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Doacao doacao)
        {

            var doacaoExistente = await _doacaoService.ObterDoacaoPorId(doacao.IdDoacao);
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
