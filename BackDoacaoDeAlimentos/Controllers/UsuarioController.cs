﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Servicos;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var usuario = await _usuarioService.ObterUsuarioPorId(id);
            return Ok(usuario);
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] LoginRequest loginRequest)
        {
            var usuario = await _usuarioService.AutenticarUsuario(loginRequest.Email, loginRequest.Senha);
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
                return BadRequest("ID do usuário não corresponde");

            await _usuarioService.AtualizarUsuario(usuario);
            return NoContent();
        }
        
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}