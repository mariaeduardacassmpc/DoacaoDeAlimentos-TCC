﻿using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IUsuarioService
    {
        Task<Entidade> ObterUsuarioPorId(int id);
        Task<Usuario> RegistrarUsuario(Usuario usuario);
        Task<Usuario> AutenticarUsuario(string email, string senha);
        Task AtualizarUsuario(Usuario usuario);
        Task<Usuario> ObterPorEmail(string email);
        public string CriptografarSenha(string senha);
    }
}