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
                throw new Exception($"Erro ao obter estatísticas.");
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
                throw new Exception($"Erro ao filtrar doações.");
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
                throw new Exception($"Erro ao obter todas as doações.");
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
                throw new Exception($"Erro ao obter doação por ID.");
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
                return CreatedAtAction(nameof(Adicionar), new { id = doacao.Id }, doacao);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar doação.");
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Doacao doacao)
        {
            try
            {
                if (doacao == null)
                    throw new ArgumentNullException(nameof(doacao));

                var doacaoExistente = await _doacaoService.ObterDoacaoPorId(doacao.Id);
                if (doacaoExistente == null)
                    return NotFound();

                await _doacaoService.AtualizarDoacao(doacao);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar doação.");
            }
        }

        [HttpPut("cancelarDoacao/{id}")]
        public async Task<IActionResult> CancelarDoacao(int id, [FromBody] string motivoCancelamento)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");

                await _doacaoService.CancelarDoacao(id, motivoCancelamento);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cancelar doação.");
            }
        }
    }
}