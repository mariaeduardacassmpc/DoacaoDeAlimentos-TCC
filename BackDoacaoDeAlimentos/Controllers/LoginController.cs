using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using BackDoacaoDeAlimentos.Repositorios;
using BackDoacaoDeAlimentos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/login")]
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

        [HttpPost("login")]
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
                TipoEntidade = usuario.Entidade?.TpEntidade
            });
        }


        [HttpPost("EnviarEmailRecuperarSenha")]
        public IActionResult RecuperarSenha([FromBody] string email)
        {
            _autenticacaoService.EnviarRecuperacaoSenha(email);
            return Ok();
        }
    }
}
