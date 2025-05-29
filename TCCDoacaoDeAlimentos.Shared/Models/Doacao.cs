using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TCCDoacaoDeAlimentos.Shared.Models 
{ 
    public class Doacao
    {
        [Key]
        public int IdDoacao { get; set; }

        [Required(ErrorMessage = "O doador é obrigatório")]
        [ForeignKey("Doador")]
        public int IdDoador { get; set; }

        [Required(ErrorMessage = "A ONG é obrigatória")]
        [ForeignKey("Ong")]
        public int IdOng { get; set; }

        [Required(ErrorMessage = "O alimento é obrigatório")]
        [ForeignKey("AlimentoDoacao")]
        public int IdAlimentoDoacao { get; set; }

        [Required(ErrorMessage = "A data da doação é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataDoacao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O status é obrigatório")]
        public StatusDoacao Status { get; set; }
        public List<AlimentoDoacao> Alimentos { get; set; } = new();
        public string? Observacao { get; set; }
    }
    public enum StatusDoacao
    {
        Pendente = 1,
        Cancelada = 2,
        Finalizada = 3
    }
}
