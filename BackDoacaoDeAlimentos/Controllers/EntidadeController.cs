using BackDoacaoDeAlimentos.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/Entidade")]  
    public class EntidadeController : ControllerBase
    {
        private readonly IEntidadeService _entidadeService;

        public EntidadeController(IEntidadeService entidadeService)
        {
            _entidadeService = entidadeService;
        }

        [HttpGet("obterTodas")]
        public async Task<IActionResult> ObterTodas()
        {
            var ongs = await _entidadeService.ObterTodasEntidades();
            return Ok(ongs);
        }

        [HttpGet("obterTodasPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var ong = await _entidadeService.ObterEntidadePorId(id);
            if (ong == null)
                return NotFound();

            return Ok(ong);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Entidade ong)
        {
            await _entidadeService.AdicionarEntidade(ong);
            return CreatedAtAction(nameof(Adicionar), new { id = ong.Id }, ong);
        }


        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Entidade ong)
        {
            if (id != ong.Id)
                return BadRequest("O ID da URL não bate com o ID do corpo.");

            var ongExistente = await _entidadeService.ObterOngPorId(id);
            if (ongExistente == null)
                return NotFound();

            await _entidadeService.AtualizarOng(ong);
            return NoContent();
        }

        [HttpDelete("deletar{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var ong = await _entidadeService.ObterOngPorId(id);
            if (ong == null)
                return NotFound();

            await _entidadeService.DeletarOng(id);
            return NoContent();
        }
    }
}
