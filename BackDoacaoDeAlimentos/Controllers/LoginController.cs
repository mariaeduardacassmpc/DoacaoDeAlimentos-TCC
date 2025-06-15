using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using BackDoacaoDeAlimentos.Repositorios;
using BackDoacaoDeAlimentos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : Controller
    {
        private IJwtService _jwtService;
        private IAutenticacaoService _autenticacaoService;
        private IUsuarioService _usuarioService; 
        private INotificacaoService _notificacaoService;
        private IEntidadeService _entidadeService;

        public LoginController(IJwtService jwtService, 
            IAutenticacaoService autenticacaoService,
            IUsuarioService usuarioService,
            INotificacaoService notificacaoService,
            IEntidadeService entidadeService
        ) 
        {
            _jwtService = jwtService;
            _autenticacaoService = autenticacaoService;
            _usuarioService = usuarioService;
            _notificacaoService = notificacaoService;
            _entidadeService = entidadeService; 
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RequisicaoLogin login)
        {
            var usuario = _usuarioService.AutenticarUsuario(login.Email, login.Senha).Result;

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            var entidade = await _entidadeService.ObterEntidadePorId(usuario.EntidadeId);
            var cidade = entidade?.Cidade ?? "";

            var token = _jwtService.GerarToken(usuario, cidade);

            return Ok(new
            {
                token,
                NomeUsuario = usuario.RazaoSocial ?? usuario.Email,
                usuario.Email,
                TpEntidade = usuario.TpEntidade,
                Cidade = cidade
            });
        }

        [HttpPost("EnviarEmailRecuperacaoSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarSenha([FromBody] RecuperacaoSenha email)
        {
            try
            {
                var resultado = await _notificacaoService.EnviarRecuperacaoSenha(email.Email);
                if (resultado)
                    return Ok("E-mail de recuperação enviado com sucesso.");
                else
                    return StatusCode(500, "Falha ao enviar e-mail de recuperação.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("RedefinirSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> RedefinirSenha([FromBody] AlterarSenhaDto redefinicao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(redefinicao.Token) || string.IsNullOrWhiteSpace(redefinicao.NovaSenha))
                    return BadRequest("Token e senha são obrigatórios.");

                await _autenticacaoService.RedefinirSenha(redefinicao.Token, redefinicao.NovaSenha);

                return Ok(new { mensagem = "Senha redefinida com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno ao redefinir senha.", detalhe = ex.Message });
            }
        }
    }
}
