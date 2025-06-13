using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using Microsoft.AspNetCore.Components.Authorization;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEntidadeRepositorio _entidadeRepositorio;

        public UsuarioService(
            IUsuarioRepositorio usuarioRepositorio,
            IEntidadeRepositorio entidadeRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _entidadeRepositorio = entidadeRepositorio;
        }

        public async Task<Entidade> ObterUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _usuarioRepositorio.ObterPorEntidadeId(id);
                if (usuario == null)
                    throw new ArgumentException("Usuário não encontrado");

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter usuário por ID.", ex);
            }
        }

        public async Task<Usuario> RegistrarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    throw new ArgumentNullException(nameof(usuario));

                if (string.IsNullOrWhiteSpace(usuario.Email))
                    throw new ArgumentException("Email é obrigatório");

                if (string.IsNullOrWhiteSpace(usuario.Senha))
                    throw new ArgumentException("Senha é obrigatória");

                if (usuario.Senha.Length < 8)
                    throw new ArgumentException("Senha deve ter no mínimo 8 caracteres");

                if (await _usuarioRepositorio.VerificarEmailExistente(usuario.Email))
                    throw new InvalidOperationException("Email já está em uso");
             
                usuario.Senha = CriptografarSenha(usuario.Senha);
                return await _usuarioRepositorio.Adicionar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar usuário.", ex);
            }
        }

        public async Task<Usuario> AutenticarUsuario(string email, string senha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email é obrigatório");

                if (string.IsNullOrWhiteSpace(senha))
                    throw new ArgumentException("Senha é obrigatória");

                var usuario = await _usuarioRepositorio.ObterPorEmail(email);
                if (usuario == null)
                    throw new ArgumentException("Usuário não encontrado");

                if (!VerificarSenha(senha, usuario.Senha))
                    throw new UnauthorizedAccessException("Senha incorreta");

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao autenticar usuário.", ex);
            }
        }


        public async Task AtualizarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    throw new ArgumentNullException(nameof(usuario));

                var usuarioExistente = await _usuarioRepositorio.ObterPorEntidadeId(usuario.Id);
                if (usuarioExistente == null)
                    throw new ArgumentException("Usuário não encontrado");

                if (!string.IsNullOrWhiteSpace(usuario.Senha) && usuario.Senha != usuarioExistente.Senha)
                {
                    usuario.Senha = CriptografarSenha(usuario.Senha);
                }
                else
                {
                    usuario.Senha = usuarioExistente.Senha;
                }

                await _usuarioRepositorio.Atualizar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar usuário.", ex);
            }
        }
     

        public string CriptografarSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }

        private bool VerificarSenha(string senhaDigitada, string senhaArmazenada)
        {
            var senhaDigitadaCriptografada = CriptografarSenha(senhaDigitada);
            return senhaDigitadaCriptografada == senhaArmazenada;
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            try
            {
                var usuario = await _usuarioRepositorio.ObterPorEmail(email);
                if (email == null)
                    throw new ArgumentException("Email não encontrado");

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter usuário por E-mail.", ex);
            }
        }
    }
}
