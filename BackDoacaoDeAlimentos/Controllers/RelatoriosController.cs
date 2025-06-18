using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;
using TCCDoacaoDeAlimentos.Shared.Dto;
using BackDoacaoDeAlimentos.Interfaces.Servicos;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/relatorio")]
    public class RelatoriosController : Controller
    {
        IRelatorioService _relatorioService;
        IDoacaoService _doacaoService;

        public RelatoriosController(IRelatorioService relatorioDeDoacoesService, IDoacaoService doacaoService)
        {
            _relatorioService = relatorioDeDoacoesService;
            _doacaoService = doacaoService;
        }

        [HttpGet("gerarPdf")]
        public async Task<IActionResult> ObterRelatorio(int idOng)
        {
            var filtro = new FiltroDoacaoDto { IdOng = idOng };
            var doacoes = (await _doacaoService.ObterDoacoesComFiltro(filtro)).ToList();

            string nomeOng = $"ONG {idOng}";
            string caminhoArquivo = $"Relatorio_Ong_{idOng}.pdf";

            _relatorioService.GerarRelatorio(
                caminhoArquivo: caminhoArquivo,
                nomeUsuario: nomeOng,
                doacoes: doacoes,
                ehDoador: false
            );

            var fileBytes = await System.IO.File.ReadAllBytesAsync(caminhoArquivo);
            return File(fileBytes, "application/pdf", Path.GetFileName(caminhoArquivo));
        }

        [HttpGet("totalDoacoesNoMes")]
        public async Task<IActionResult> ObterTotalDoacoesMes(int id)
        {
            try
            {
                var total = await _relatorioService.ObterTotalDoacoesMes(id);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter total de doações do mês: {ex.Message}");
            }
        }

        [HttpGet("totalUsuarios")]
        public async Task<IActionResult> ObterTotalUsuarios()
        {
            try
            {
                var total = await _relatorioService.ObterTotalUsuarios();
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter total de usuários: {ex.Message}");
            }
        }

        [HttpGet("alimentosPorCategoria")]
        public async Task<IActionResult> ObterAlimentosPorCategoria(int id)
        {
            try
            {
                var lista = await _relatorioService.ObterAlimentosPorCategoria(id);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter alimentos por categoria: {ex.Message}");
            }
        }

        [HttpGet("totalKgAlimentos")]
        public async Task<IActionResult> ObterTotalKgAlimentos(int id)
        {
            try
            {
                var total = await _relatorioService.ObterTotalKgAlimentos(id);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter total de kg de alimentos: {ex.Message}");
            }
        }
    }
}

