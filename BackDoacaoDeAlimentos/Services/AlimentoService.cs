using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
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

        public Task<IEnumerable<Alimento>> ObterTodosAlimentos()
        {
            try
            {
                return _alimentoRepositorio.ObterTodosAlimentos();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os alimentos.", ex);
            }
        }

        public Task<Alimento> ObterAlimentoPorId(int id)
        {
            try
            {
                return _alimentoRepositorio.ObterAlimentoPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter o alimento com ID {id}.", ex);
            }
        }

        public Alimento AdicionarAlimento(Alimento alimento)
        {
            try
            {
                _alimentoRepositorio.AdicionarAlimento(alimento);
                return alimento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o alimento.", ex);
            }
        }

        public void RemoverAlimento(int id)
        {
            try
            {
                _alimentoRepositorio.RemoverAlimento(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover o alimento com ID {id}.", ex);
            }
        }
    }
}
