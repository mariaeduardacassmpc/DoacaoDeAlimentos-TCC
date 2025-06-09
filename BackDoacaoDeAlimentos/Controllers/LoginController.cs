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

        public LoginController(IJwtService jwtService, 
            IAutenticacaoService autenticacaoService,
            IUsuarioService usuarioService
        ) 
        {
            _jwtService = jwtService;
            _autenticacaoService = autenticacaoService;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _usuarioService.AutenticarUsuario(request.Email, request.Senha).Result;

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            var token = _jwtService.GerarToken(usuario);

            return Ok(new
            {
                token,
                NomeUsuario = usuario.Entidade?.RazaoSocial ?? usuario.Email,
                usuario.Email,
                TpEntidade = usuario.TpEntidade
            });
        }


        [HttpPost("EnviarEmailRecuperacaoSenha")]
        public async Task<IActionResult> RecuperarSenha([FromBody] RecuperacaoSenha email)
        {
            try
            {
                var resultado = await _autenticacaoService.EnviarRecuperacaoSenha(email.Email);
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
    }
}
