using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared;
using BackDoacaoDeAlimentos.Servicos;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Servicos;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/entidade")]  
    public class EntidadeController : ControllerBase
    {
        private readonly IEntidadeService _entidadeService;
        private readonly IUsuarioService _usuarioService;
        private readonly IGeolocalizacaoService _geocodingService;

        public EntidadeController(
            IEntidadeService entidadeService,
            IUsuarioService usuarioService,
            IGeolocalizacaoService geocodingService)
        {
            _entidadeService = entidadeService;
            _usuarioService = usuarioService;
            _geocodingService = geocodingService;
        }


        [HttpGet("obterTodasInstituicoes")]
        public async Task<IActionResult> ObterTodasInstituicoes()
        {
            var entidades = await _entidadeService.ObterTodasInstituicoes();
            if (entidades == null)
                return NotFound();

            return Ok(entidades);
        }

        [HttpGet("obterTodasEntidades")]
        public async Task<IActionResult> ObterTodasEntidades()
        {
            var entidades = await _entidadeService.ObterTodasEntidades();
            if (entidades == null)
                return NotFound();

            return Ok(entidades);
        }

        [HttpGet("obterPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var entidade = await _entidadeService.ObterEntidadePorId(id);
            if (entidade == null)
                return NotFound();

            return Ok(entidade);
        }

        [HttpGet("obterPorEmail/{email}")]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            var entidade = await _usuarioService.ObterPorEmail(email);
            if (entidade == null)
                return NotFound();

            return Ok(entidade);
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Entidade entidade)
        {
            try
            {
                var usuario = new Usuario
                {
                    Email = entidade.Email,
                    Senha = entidade.Senha
                };

                await _entidadeService.AdicionarEntidade(entidade, usuario);

                return Ok(new { success = true, message = "Cadastro realizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] EntidadeEdicaoDto entidade)
        {
            if (entidade == null || entidade.Id != id)
                return BadRequest();

            await _entidadeService.AtualizarEntidade(entidade);
            return NoContent();
        }

        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> Inativar(int id)
        {
            var entidade = await _entidadeService.ObterEntidadePorId(id);
            if (entidade == null)
                return NotFound();

            await _entidadeService.InativarEntidade(id);

            return NoContent();
        }

        [HttpGet("verificarDocumentoeEmail/{documento}/{email}")]
        public async Task<IActionResult> VerificarDocumentoeEmailExistente(string documento, string email)
        {
            var existe = await _entidadeService.VerificarDocumentoeEmailExistente(documento, email);
            return Ok(new { existe });
        }


        [HttpGet("buscarInstituicoesPorCoordenadas/{latitude}/{longitude}")]
        public async Task<IActionResult> BuscarInstituicaoPorCoordenadas(string latitude, string longitude)
        {
            if (!double.TryParse(latitude, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat) ||
                !double.TryParse(longitude, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
            {
                return BadRequest("Latitude ou longitude em formato inválido.");
            }

            var cidade = await _geocodingService.ObterCidadePorCoordenadas(lat, lon);

            if (string.IsNullOrWhiteSpace(cidade))
                return NotFound("Cidade não encontrada para as coordenadas informadas.");

            var instituicao = await _entidadeService.ObterInstituicaoPorCidade(cidade);

            return Ok(instituicao);
        }

        [HttpGet("buscarInstituicoesPorCidade/{cidade}")]
        public async Task<IActionResult> BuscarInstituicaoPorCoordenadas(string cidade)
        {
            var instituicao = await _entidadeService.ObterInstituicaoPorCidade(cidade);
            return Ok(instituicao);
        }
    }
}
