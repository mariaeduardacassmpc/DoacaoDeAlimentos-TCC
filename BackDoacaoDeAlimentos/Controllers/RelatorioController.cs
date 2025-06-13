using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/Relatorio")]
    public class RelatorioController : Controller
    {
        RelatorioDeDoacoesService _relatorioDeDoacoesService;
        IDoacaoService _doacaoService;

        public RelatorioController(RelatorioDeDoacoesService relatorioDeDoacoesService, IDoacaoService doacaoService)
        {
            _relatorioDeDoacoesService = relatorioDeDoacoesService;
            _doacaoService = doacaoService;
        }

        [HttpGet("gerarPdf")]
        public async Task<IActionResult> ObterRelatorio(int idOng)
        {
            var filtro = new FiltroDoacaoDto { IdOng = idOng };
            var doacoes = (await _doacaoService.ObterDoacoesComFiltro(filtro)).ToList();

            string nomeOng = $"ONG {idOng}";
            string caminhoArquivo = $"Relatorio_Ong_{idOng}.pdf";

            _relatorioDeDoacoesService.GerarRelatorio(
                caminhoArquivo: caminhoArquivo,
                nomeUsuario: nomeOng,
                doacoes: doacoes,
                ehDoador: false
            );

            var fileBytes = await System.IO.File.ReadAllBytesAsync(caminhoArquivo);
            return File(fileBytes, "application/pdf", Path.GetFileName(caminhoArquivo));
        }
    }
}
