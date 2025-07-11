﻿using System.Text;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using Microsoft.AspNetCore.Identity;
using MimeKit;
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
        private readonly IPasswordHasher<Usuario> _hasher;
        private readonly IUsuarioService _usuarioService;

        public AutenticacaoService(
              IEntidadeRepositorio entidadeRepositorio,
              HttpClient http,
              IMailService mailServico,
              IUsuarioRepositorio usuarioRepositorio,
              IJwtService jwtService,
              IPasswordHasher<Usuario> hasher,
              IUsuarioService usuarioService
        )
        {
            _entidadeRepositorio = entidadeRepositorio;
            _http = http;
            _mailServico = mailServico;
            _usuarioRepositorio = usuarioRepositorio;
            _jwtService = jwtService;
            _hasher = hasher;
            _usuarioService = usuarioService;
        }


        public string GerarHashSenha(string senha)
        {
            try
            {
                return _hasher.HashPassword(null, senha);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar hash da senha.");
            }
        }

        public bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            try
            {
                var resultado = _hasher.VerifyHashedPassword(null, senhaHash, senhaDigitada);
                return resultado == PasswordVerificationResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar senha.");
            }
        }

        public void Login(string login, string senha)
        {
            try
            {
                var usuarioTask = _usuarioRepositorio.ObterPorEmail(login);
                var usuario = usuarioTask.Result;

                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");

                if (!VerificarSenha(senha, usuario.SenhaHash))
                    throw new Exception("Senha inválida.");

                //var token = _jwtService.GerarToken(usuario);

                //return new RespostaAutenticacao
                //{
                //    Token = token,
                //    NomeUsuario = usuario.RazaoSocial ?? usuario.Email,
                //    Email = usuario.Email
                //};
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no processo de login.");
            }
        }

        public async Task Registrar(string nome, string login, string senha)
        {
            try
            {
                var existe = await _usuarioRepositorio.VerificarEmailExistente(login);
                if (existe)
                    throw new Exception("Usuário já cadastrado.");

                var senhaHash = GerarHashSenha(senha);

                // Descomente e complete se for adicionar o usuário
                // var usuario = new Usuario
                // {
                //     Email = login,
                //     SenhaHash = senhaHash
                // };
                // await _usuarioRepositorio.Adicionar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar usuário.");
            }
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

                if (DateTime.Now > dataExpiracao)
                    return (false, null);

                return (true, email);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task RedefinirSenha(string token, string novaSenha)
        {
            var (valido, email) = ValidarToken(token); 
            if (!valido || string.IsNullOrEmpty(email))
                throw new Exception("Token inválido ou expirado.");

            Usuario usuario = await _usuarioRepositorio.ObterPorEmail(email);
            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            var senhaHash = _usuarioService.CriptografarSenha(novaSenha);
            usuario.Senha = senhaHash;
            await _usuarioRepositorio.Atualizar(usuario);
        }
    }
}
