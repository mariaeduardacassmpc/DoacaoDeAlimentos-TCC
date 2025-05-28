using System.Text.Json;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Servicos;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;


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

        [HttpGet("cidade/{cidade}")]
        public async Task<IActionResult> GetOngsPorCidade(string cidade)
        {
            var ongs = await _entidadeService.BuscarOngsPorCidade(cidade);
            return Ok(ongs);
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

        [HttpGet("verificar-cpf-cnpj/{documento}")]
        public async Task<IActionResult> VerificarCpfCnpjExistente(string documento)
        {
            var existe = await _entidadeService.VerificarCpfCnpjExistente(documento);
            return Ok(new { existe });
        }
    }
}
