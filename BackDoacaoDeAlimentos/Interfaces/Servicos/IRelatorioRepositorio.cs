using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IRelatorioService
    {
        Task<int> ObterTotalDoacoesMes(int id);
        Task<int> ObterTotalUsuarios();
        Task<IEnumerable<AlimentoPorCategoriaDto>> ObterAlimentosPorCategoria(int id);
        Task<double> ObterTotalKgAlimentos(int id);
        string GetStatusMaisComum(List<Doacao> doacoes);
        void GerarRelatorio(string caminhoArquivo, string nomeUsuario, List<Doacao> doacoes, bool ehDoador);

    }
}
