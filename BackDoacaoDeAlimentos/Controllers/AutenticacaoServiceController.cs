using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Servicos;

[ApiController]
[Route("api/autenticacao")]
public class AutenticacaoServiceController : ControllerBase
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IEntidadeRepositorio _entidadeRepositorio;
    private readonly IMailService _mailServico;

    public AutenticacaoServiceController(
        IUsuarioRepositorio usuarioRepositorio,
        IEntidadeRepositorio entidadeRepositorio,
        IMailService mailServico)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _entidadeRepositorio = entidadeRepositorio;
        _mailServico = mailServico;
    }

    [HttpPost("enviarRecuperacaoSenha")]
    public async Task<IActionResult> EnviarRecuperacaoSenha([FromBody] string email)
    {
        try
        {
            var resultado = await EnviarRecuperacaoSenhaInterno(email);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    private async Task<bool> EnviarRecuperacaoSenhaInterno(string email)
    {
        Usuario usuario = await _usuarioRepositorio.ObterPorEmail(email);
        if (usuario == null)
            throw new Exception("Usuário não encontrado.");

        Entidade entidade = await _entidadeRepositorio.ObterEntidadePorId(usuario.EntidadeId);
        if (entidade == null)
            throw new Exception("Entidade não encontrada.");

        var token = GerarTokenRecuperacao(email);
        var link = $"https://seusite.com/recuperar-senha?token={token}";

        return await _mailServico.EnviarEmailRecuperacaoSenha(
            entidade.RazaoSocial,
            usuario.Email,
            usuario.Email,
            link
        );
    }

    private string GerarTokenRecuperacao(string email)
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
