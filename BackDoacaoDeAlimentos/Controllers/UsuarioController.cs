using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Servicos;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObterUsuarioPorId(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar usuário: {ex.Message}");
            }
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] RequisicaoLogin login)
        {
            try
            {
                var usuario = await _usuarioService.AutenticarUsuario(login.Email, login.Senha);
                if (usuario == null)
                    return Unauthorized("Credenciais inválidas.");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao autenticar usuário: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (id != usuario.Id)
                    return BadRequest("ID do usuário não corresponde.");

                await _usuarioService.AtualizarUsuario(usuario);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar usuário: {ex.Message}");
            }
        }
    }
}