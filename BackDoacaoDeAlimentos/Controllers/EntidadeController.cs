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
            var entidades = await _entidadeService.ObterTodasEntidades();
            if (entidades == null)
                return NotFound();

            return Ok(entidades);
        }

        [HttpGet("obterTodasPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var entidade = await _entidadeService.ObterEntidadePorId(id);
            if (entidade == null)
                return NotFound();

            return Ok(entidade);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Entidade entidade)
        {
            await _entidadeService.AdicionarEntidade(entidade);
            return CreatedAtAction(nameof(Adicionar), new { id = entidade.Id }, entidade);
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Entidade entidade)
        {
            var entidadeExistente = await _entidadeService.ObterEntidadePorId(entidade.Id);
            if (entidadeExistente == null)
                return NotFound();

            await _entidadeService.AtualizarEntidade(entidade);
            return NoContent();
        }

        [HttpDelete("deletar{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var entidade = await _entidadeService.ObterEntidadePorId(id);
            if (entidade == null)
                return NotFound();

            await _entidadeService.DeletarEntidade(id);
            return NoContent();
        }
    }
}
