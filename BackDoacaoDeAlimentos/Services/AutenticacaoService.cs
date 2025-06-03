using System.Text;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using Microsoft.AspNetCore.Identity;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IEntidadeRepositorio _entidadeRepositorio;
        private readonly IJwtService _jwtService;
        private readonly HttpClient _http;
        private readonly IMailService _mailServico;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPasswordHasher<object> _hasher;

        public AutenticacaoService(
            IEntidadeRepositorio entidadeRepositorio,
            HttpClient http,
            IMailService mailServico,
            IUsuarioRepositorio usuarioRepositorio,
            IJwtService jwtService,
            IPasswordHasher<object> hasher)

        {
            _entidadeRepositorio = entidadeRepositorio;
            _http = http;
            _mailServico = mailServico;
            _usuarioRepositorio = usuarioRepositorio;
            _jwtService = jwtService;
            _hasher = hasher;
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
            var usuarioTask = _usuarioRepositorio.ObterPorEmail(login);
            var usuario = usuarioTask.Result; 

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            if (!VerificarSenha(senha, usuario.SenhaHash))
                throw new Exception("Senha inválida.");

            var token = _jwtService.GerarToken(usuario);

            return new RespostaAutenticacao
            {
                Token = token,
                NomeUsuario = usuario.Entidade?.RazaoSocial ?? usuario.Email,
                Email = usuario.Email
            };
        }

        public async Task Registrar(string nome, string login, string senha)
        {
            var existe = await _usuarioRepositorio.VerificarEmailExistente(login);
            if (existe)
                throw new Exception("Usuário já cadastrado.");

            var senhaHash = GerarHashSenha(senha);

            var usuario = new Usuario
            {
                Email = login,
                SenhaHash = senhaHash
            };

            await _usuarioRepositorio.Adicionar(usuario);
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
