using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> ObterEstatisticas()
        {
            try
            {
                var doacoes = await _doacaoService.ObterEstatisticas();
                if (doacoes == null)
                    return Ok(new EstatisticasDto { TotalOngsAjudadas = 0, TotalKgAlimentosDoado = 0 });

                return Ok(doacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter estatísticas: {ex.Message}", ex);
            }
        }

        [HttpPost("filtrarDoacao")]
        public async Task<IActionResult> ObterTodasPorEntidade([FromBody] FiltroDoacaoDto filtroDoacaoDto)
        {
            try
            {
                if (filtroDoacaoDto == null)
                    throw new ArgumentNullException(nameof(filtroDoacaoDto));

                var doacoes = await _doacaoService.ObterDoacoesComFiltro(filtroDoacaoDto);
                if (doacoes == null)
                    return NotFound();

                return Ok(doacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao filtrar doações: {ex.Message}", ex);
            }
        }

        [HttpGet("obterTodasDoacoes")]
        public async Task<IActionResult> ObterTodas()
        {
            try
            {
                var doacoes = await _doacaoService.ObterTodasDoacoes();
                if (doacoes == null)
                    return NotFound();

                return Ok(doacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter todas as doações: {ex.Message}", ex);
            }
        }

        [HttpGet("obterTodasPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");

                var doacao = await _doacaoService.ObterDoacaoPorId(id);
                if (doacao == null)
                    return NotFound();

                return Ok(doacao);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter doação por ID: {ex.Message}", ex);
            }
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Doacao doacao)
        {
            try
            {
                if (doacao == null)
                    throw new ArgumentNullException(nameof(doacao));

                await _doacaoService.AdicionarDoacao(doacao);
                return CreatedAtAction(nameof(Adicionar), new { id = doacao.IdDoacao }, doacao);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar doação: {ex.Message}", ex);
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Doacao doacao)
        {
            try
            {
                if (doacao == null)
                    throw new ArgumentNullException(nameof(doacao));

                var doacaoExistente = await _doacaoService.ObterDoacaoPorId(doacao.IdDoacao);
                if (doacaoExistente == null)
                    return NotFound();

                await _doacaoService.AtualizarDoacao(doacao);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar doação: {ex.Message}", ex);
            }
        }

        [HttpDelete("CancelarDoacao/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");

                var doacao = await _doacaoService.ObterDoacaoPorId(id);
                if (doacao == null)
                    return NotFound();

                await _doacaoService.DeletarDoacao(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cancelar doação: {ex.Message}", ex);
            }
        }
    }
}