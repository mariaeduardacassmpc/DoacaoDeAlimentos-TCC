using System.Data;
using System.Text;
using Blazored.LocalStorage; 
using System.Net.Http.Headers;
using System.Security.Cryptography;
using BackDoacaoDeAlimentos.Repositorios;
using TCCDoacaoDeAlimentos.Shared.Models;
using static System.Net.WebRequestMethods;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using Microsoft.AspNetCore.Identity;

namespace BackDoacaoDeAlimentos.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private IEntidadeRepositorio _entidadeRepositorio;
        private readonly HttpClient _http;
        private IMailService _mailServico;
        private IUsuarioRepositorio _usuarioRepositorio;
        private readonly PasswordHasher<object> _hasher = new();

        public AutenticacaoService(IEntidadeRepositorio entidadeRepositorio, 
            HttpClient http,
            IMailService mailServico,
            IUsuarioRepositorio usuarioRepositorio) 
        {
            _entidadeRepositorio = entidadeRepositorio;
            _http = http;
            _mailServico = mailServico;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public string GerarHashSenha(string senha)
        {
            return _hasher.HashPassword(null, senha);
        }

        public bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            var resultado = _hasher.VerifyHashedPassword(null, senhaHash, senhaDigitada);
            return resultado == PasswordVerificationResult.Success;
        }

        public RespostaAutenticacao Login(string login, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Login == login);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            if (!VerificarSenha(senha, usuario.SenhaHash))
                throw new Exception("Senha inválida.");

            var token = _jwtService.GerarToken(usuario);

            return new RespostaAutenticacao
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Login
            };

        }
        public void Registrar(string nome, string login, string senha)
        {
            var existe = _context.Usuarios.Any(u => u.Login == login);
            if (existe)
                throw new Exception("Usuário já cadastrado.");

            var senhaHash = GerarHashSenha(senha);

            var usuario = new Usuario
            {
                Nome = nome,
                Login = login,
                SenhaHash = senhaHash
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _http.DefaultRequestHeaders.Authorization = null;
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
