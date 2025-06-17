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
            try
            {
                var entidades = await _entidadeService.ObterTodasInstituicoes();
                if (entidades == null)
                    return NotFound();

                return Ok(entidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao obter instituições.", details = ex.Message });
            }
        }

        [HttpGet("obterTodasEntidades")]
        public async Task<IActionResult> ObterTodasEntidades()
        {
            try
            {
                var entidades = await _entidadeService.ObterTodasEntidades();
                if (entidades == null)
                    return NotFound();

                return Ok(entidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao obter entidades.", details = ex.Message });
            }
        }

        [HttpGet("obterPorId/{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var entidade = await _entidadeService.ObterEntidadePorId(id);
                if (entidade == null)
                    return NotFound();

                return Ok(entidade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao obter entidade por ID.", details = ex.Message });
            }
        }

        [HttpGet("obterPorEmail/{email}")]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            try
            {
                var entidade = await _usuarioService.ObterPorEmail(email);
                if (entidade == null)
                    return NotFound();

                return Ok(entidade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao obter entidade por e-mail.", details = ex.Message });
            }
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
                return StatusCode(500, new { success = false, message = "Erro ao adicionar entidade.", details = ex.Message });
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] EntidadeEdicaoDto entidade)
        {
            try
            {
                if (entidade == null || entidade.Id != id)
                    return BadRequest();

                await _entidadeService.AtualizarEntidade(entidade);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao atualizar entidade.", details = ex.Message });
            }
        }

        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> Inativar(int id)
        {
            try
            {
                var entidade = await _entidadeService.ObterEntidadePorId(id);
                if (entidade == null)
                    return NotFound();

                await _entidadeService.InativarEntidade(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao inativar entidade.", details = ex.Message });
            }
        }

        [HttpGet("verificarDocumentoeEmail/{documento}/{email}")]
        public async Task<IActionResult> VerificarDocumentoeEmailExistente(string documento, string email)
        {
            try
            {
                var existe = await _entidadeService.VerificarDocumentoeEmailExistente(documento, email);
                return Ok(new { existe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao verificar documento e e-mail.", details = ex.Message });
            }
        }

        [HttpGet("buscarInstituicoesPorCoordenadas/{latitude}/{longitude}")]
        public async Task<IActionResult> BuscarInstituicaoPorCoordenadas(string latitude, string longitude)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar instituições por coordenadas.", details = ex.Message });
            }
        }

        [HttpGet("buscarInstituicoesPorCidade/{cidade}")]
        public async Task<IActionResult> BuscarInstituicaoPorCoordenadas(string cidade)
        {
            try
            {
                var instituicao = await _entidadeService.ObterInstituicaoPorCidade(cidade);
                return Ok(instituicao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar instituições por cidade.", details = ex.Message });
            }
        }
    }
}
