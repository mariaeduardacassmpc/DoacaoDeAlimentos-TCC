using BackDoacaoDeAlimentos.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/Ong")]
    public class OngController : ControllerBase
    {
        private readonly IOngService _ongService;

        public OngController(IOngService ongService)
        {
            _ongService = ongService;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Entidade ong)
        {
            await _ongService.AdicionarOng(ong);
            return CreatedAtAction(nameof(Adicionar), new { id = ong.Id}, ong);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> ObterPorId(int id)
        //{
        //    var ong = await _ongService.ObterOngPorId(id);
        //    if (ong == null)
        //        return NotFound();

        //    return Ok(ong);
        //}

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var ongs = await _ongService.ObterTodasOngs();
            return Ok(ongs);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Atualizar(int id, [FromBody] Ong ong)
        //{
        //    if (id != ong.Id)
        //        return BadRequest("O ID da URL não bate com o ID do corpo.");

        //    var ongExistente = await _ongService.ObterOngPorId(id);
        //    if (ongExistente == null)
        //        return NotFound();

        //    await _ongService.AtualizarOng(ong);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Deletar(int id)
        //{
        //    var ong = await _ongService.ObterOngPorId(id);
        //    if (ong == null)
        //        return NotFound();

        //    await _ongService.DeletarOng(id);
        //    return NoContent();
        //}
    }
}
