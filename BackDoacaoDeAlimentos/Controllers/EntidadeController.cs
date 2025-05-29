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


        public EntidadeController(IEntidadeService entidadeService, IUsuarioService usuarioService)
        {
            _entidadeService = entidadeService;
            _usuarioService = usuarioService;

        }

        [HttpGet("buscarOngsPorCidade/{cidade}")]
        public async Task<IActionResult> BuscarOngsPorCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade inválida.");

            var ongs = await _entidadeService.ObterOngsPorCidade(cidade);
            return Ok(ongs);
        }

        [HttpGet("obterTodasOngs")]
        public async Task<IActionResult> ObterTodasOngs()
        {
            var entidades = await _entidadeService.ObterTodasOngs();
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

        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] Entidade entidadeDto)
        {
            try
            {
                var entidade = new Entidade
                {
                    TpEntidade = entidadeDto.TpEntidade,
                    RazaoSocial = entidadeDto.RazaoSocial,
                    Senha = entidadeDto.Senha,
                    ConfirmarSenha = entidadeDto.ConfirmarSenha,
                    CNPJ_CPF = entidadeDto.CNPJ_CPF,
                    Telefone = entidadeDto.Telefone,
                    Endereco = entidadeDto.Endereco,
                    Bairro = entidadeDto.Bairro,
                    CEP = entidadeDto.CEP,
                    Cidade = entidadeDto.Cidade,
                    Email = entidadeDto.Email,
                    Responsavel = entidadeDto.Responsavel,
                    Sexo = entidadeDto.Sexo,
                    Latitude = entidadeDto.Latitude,
                    Altitude = entidadeDto.Altitude
                };

                var usuario = new Usuario
                {
                    Email = entidadeDto.Email,
                    Senha = entidadeDto.Senha
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
        public async Task<IActionResult> Atualizar([FromBody] EntidadeEdicaoDto entidade)
        {
            if (entidade == null)
                return NotFound();

            await _entidadeService.AtualizarEntidade(entidade);
            return NoContent();
        }


        [HttpDelete("deletar/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var entidade = await _entidadeService.ObterEntidadePorId(id);
            if (entidade == null)
                return NotFound();

            await _entidadeService.DeletarEntidade(id);
            return NoContent();
        }

        [HttpGet("verificarDocumentoeEmail/{documento}/{email}")]
        public async Task<IActionResult> VerificarDocumentoeEmailExistente(string documento, string email)
        {
            var existe = await _entidadeService.VerificarDocumentoeEmailExistente(documento, email);
            return Ok(new { existe });
        }
    }
}
