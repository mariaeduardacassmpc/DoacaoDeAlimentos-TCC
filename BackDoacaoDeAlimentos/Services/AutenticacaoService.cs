using System.Data;
using System.Security.Cryptography;
using System.Text;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorios;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private IEntidadeRepositorio _entidadeRepositorio;
        private IUsuarioRepositorio _usuarioRepositorio;
        private IMailService _mailServico;

        public AutenticacaoService(IEntidadeRepositorio entidadeRepositorio, 
            IMailService mailServico, 
            IUsuarioRepositorio usuarioRepositorio) 
        {
            _entidadeRepositorio = entidadeRepositorio;
            _mailServico = mailServico;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<bool> EnviarRecuperacaoSenha(string email)
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
            var dataExpiracao = DateTime.UtcNow.AddMinutes(30); 
            var payload = $"{email}|{dataExpiracao:o}";

            var bytes = Encoding.UTF8.GetBytes(payload);
            var token = Convert.ToBase64String(bytes);

            return Uri.EscapeDataString(token); 
        }

        public (bool valido, string email) ValidarToken(string token)
        {
            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(Uri.UnescapeDataString(token)));
                var partes = decoded.Split('|');

                if (partes.Length != 2)
                    return (false, null);

                var email = partes[0];
                var dataExpiracao = DateTime.Parse(partes[1]);

                if (DateTime.UtcNow > dataExpiracao)
                    return (false, null);

                return (true, email);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
