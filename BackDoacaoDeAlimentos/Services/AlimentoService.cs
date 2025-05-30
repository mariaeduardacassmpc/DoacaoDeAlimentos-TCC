using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorios;
using Microsoft.AspNetCore.Http.HttpResults;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class AlimentoService : IAlimentoService
    {
        private readonly IAlimentoRepositorio _alimentoRepositorio;

        public AlimentoService(IAlimentoRepositorio alimentoRepositorio)
        {
            _alimentoRepositorio = alimentoRepositorio;
        }

        public async Task<IEnumerable<Alimento>> ObterTodosAlimentos()
        {
            try
            {
                return await _alimentoRepositorio.ObterTodosAlimentos();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os Alimento.", ex);
            }
        }

        public async Task<Alimento> ObterAlimentoPorId(int id)
        {
            try
            {
                return await _alimentoRepositorio.ObterAlimentoPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter o Alimento.", ex);
            }
        }

        public async Task AdicionarAlimento(Alimento alimento)
        {
            try
            {
                await _alimentoRepositorio.AdicionarAlimento(alimento);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o Alimento.", ex);
            }
        }

        public async Task InativarAlimento(int id)
        {
            try
            {
                await _alimentoRepositorio.InativarAlimento(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover o Alimento.", ex);
            }
        }


        public async Task AtualizarAlimento(Alimento alimento)
        {
            try
            {
                await _alimentoRepositorio.AtualizarAlimentos(alimento);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar Alimento. ", ex);
            }
        }
    }
}
