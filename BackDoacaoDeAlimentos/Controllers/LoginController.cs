using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using BackDoacaoDeAlimentos.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackDoacaoDeAlimentos.Controllers
{
    public class LoginController : Controller
    {
        private IAutenticacaoService _autenticacaoService;
        public LoginController(IAutenticacaoService autenticacaoService) 
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("EnviarEmailRecuperarSenha")]
        public IActionResult RecuperarSenha([FromBody] string email)
        {
            _autenticacaoService.EnviarRecuperacaoSenha(email);
            return Ok();
        }
    }
}
