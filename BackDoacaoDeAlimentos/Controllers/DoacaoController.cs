using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doacao>>> ObterTodas()
        {
            var doacoes = await _doacaoService.ObteerTodasDoacoes();
            return Ok(doacoes);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Doacao>> ObterPorId(int id)
        //{
        //    var doacao = await _doacaoService.(id);
        //    if (doacao == null)
        //        return NotFound();

        //    return Ok(doacao);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Criar([FromBody] CriarDoacaoDto dto)
        //{
        //    var id = await _doacaoService.CriarAsync(dto);
        //    return CreatedAtAction(nameof(ObterPorId), new { id }, null);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Atualizar(int id, [FromBody] AtualizarDoacao dto)
        //{
        //    var sucesso = await _doacaoService.Atualizar(id, dto);
        //    if (!sucesso)
        //        return NotFound();

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Deletar(int id)
        //{
        //    var sucesso = await _doacaoService.DeletarAsync(id);
        //    if (!sucesso)
        //        return NotFound();

        //    return NoContent();
        //}
    }
}
