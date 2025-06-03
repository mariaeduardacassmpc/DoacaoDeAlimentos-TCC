using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using BackDoacaoDeAlimentos.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Controllers
{
    public class LoginController : Controller
    {
        private IAutenticacaoService _autenticacaoService;
        public LoginController(IAutenticacaoService autenticacaoService) 
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == request.Email);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, request.Senha);

            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Senha inválida.");

            var token = GenerateToken(usuario);

            return Ok(new
            {
                token,
                usuario.Nome,
                usuario.Email
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
