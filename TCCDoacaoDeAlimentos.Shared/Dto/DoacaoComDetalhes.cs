namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class DoacaoComDetalhes
    {
        public int Id { get; set; }
        public DateTime DataDoacao { get; set; }
        public StatusDoacao Status { get; set; }

        public int Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
        public DateTime Validade { get; set; }

        public int AlimentoId { get; set; }
        public string AlimentoNome { get; set; }

        public string DoadorNome { get; set; }

        public string OngNome { get; set; }

        public enum StatusDoacao
        {
            Pendente = 1,
            Cancelada = 2,
            Finalizada = 3
        }
    }
}
