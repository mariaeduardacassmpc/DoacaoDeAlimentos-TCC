using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class AlimentoDoacaoService : IAlimentoDoacaoService
    {
        private readonly IAlimentoDoacaoRepositorio _alimentoDoacaoRepo;
        private readonly IAlimentoRepositorio _alimentoRepositorio;
        private readonly IDoacaoRepositorio _doacaoRepositorio;

        public AlimentoDoacaoService(
            IAlimentoDoacaoRepositorio alimentoDoacaoRepo,
            IAlimentoRepositorio alimentoRepo,
            IDoacaoRepositorio doacaoRepo)
        {
            _alimentoDoacaoRepo = alimentoDoacaoRepo;
            _alimentoRepositorio = alimentoRepo;
            _doacaoRepositorio = doacaoRepo;
        }

        public async Task<AlimentoDoacao> AdicionarAlimentoADoacaoAsync(AlimentoDoacao alimentoDoacao)
        {
            try
            {
                await ValidarAlimentoDoacaoAsync(alimentoDoacao);

                var doacao = await _doacaoRepositorio.ObterDoacaoPorId(alimentoDoacao.IdDoacao);
                if (doacao == null)
                    throw new ArgumentException("Doação não encontrada");

                if (doacao.Status != StatusDoacao.Pendente)
                    throw new InvalidOperationException("Só é possível adicionar alimentos a doações pendentes");

                return await _alimentoDoacaoRepo.AdicionarAsync(alimentoDoacao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar alimento à doação", ex);
            }
        }

        public async Task RemoverAlimentoDeDoacaoAsync(int id)
        {
            try
            {
                var alimentoDoacao = await _alimentoDoacaoRepo.ObterPorIdAsync(id);
                if (alimentoDoacao == null)
                    throw new ArgumentException("Relação alimento-doação não encontrada");

                var doacao = await _doacaoRepositorio.ObterDoacaoPorId(alimentoDoacao.IdDoacao);
                if (doacao.Status != StatusDoacao.Pendente)
                    throw new InvalidOperationException("Só é possível remover alimentos de doações pendentes");

                await _alimentoDoacaoRepo.RemoverAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover alimento da doação", ex);
            }
        }

        public async Task<IEnumerable<AlimentoDoacao>> ListarAlimentosPorDoacaoAsync(int doacaoId)
        {
            try
            {
                return await _alimentoDoacaoRepo.ObterPorDoacaoAsync(doacaoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar alimentos da doação", ex);
            }
        }

        public async Task<IEnumerable<AlimentoDoacao>> ListarAlimentosProximosVencimentoAsync(int diasAntecedencia)
        {
            try
            {
                var todos = await _alimentoDoacaoRepo.ObterTodosAsync();
                var dataLimite = DateTime.Today.AddDays(diasAntecedencia);
                return todos
                    .Where(a => a.Validade <= dataLimite && a.Validade >= DateTime.Today)
                    .OrderBy(a => a.Validade);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar alimentos próximos do vencimento", ex);
            }
        }

        public async Task AtualizarQuantidadeAlimentoAsync(int id, decimal novaQuantidade)
        {
            try
            {
                if (novaQuantidade <= 0)
                    throw new ArgumentException("Quantidade deve ser maior que zero");

                var alimentoDoacao = await _alimentoDoacaoRepo.ObterPorIdAsync(id);
                if (alimentoDoacao == null)
                    throw new ArgumentException("Relação alimento-doação não encontrada");

                var doacao = await _doacaoRepositorio.ObterDoacaoPorId(alimentoDoacao.IdDoacao);
                if (doacao.Status != StatusDoacao.Pendente)
                    throw new InvalidOperationException("Só é possível alterar quantidades em doações pendentes");

                alimentoDoacao.Quantidade = novaQuantidade;
                await _alimentoDoacaoRepo.AtualizarAsync(alimentoDoacao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar quantidade do alimento", ex);
            }
        }

        public async Task<AlimentoDoacao> ObterPorIdAsync(int id)
        {
            try
            {
                return await _alimentoDoacaoRepo.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter alimento da doação por ID", ex);
            }
        }

        public async Task ValidarAlimentoDoacaoAsync(AlimentoDoacao alimentoDoacao)
        {
            try
            {
                if (alimentoDoacao == null)
                    throw new ArgumentNullException(nameof(alimentoDoacao));

                if (alimentoDoacao.Validade < DateTime.Today)
                    throw new ArgumentException("Alimento com validade vencida");

                if (alimentoDoacao.Quantidade <= 0)
                    throw new ArgumentException("A quantidade deve ser maior que zero");

                var alimento = await _alimentoRepositorio.ObterAlimentoPorId(alimentoDoacao.AlimentoId);
                if (alimento == null)
                    throw new ArgumentException("Alimento não encontrado");

                if (string.IsNullOrWhiteSpace(alimentoDoacao.UnidadeMedida))
                    throw new ArgumentException("Unidade de medida é obrigatória");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao validar alimento da doação", ex);
            }
        }
    }
}
