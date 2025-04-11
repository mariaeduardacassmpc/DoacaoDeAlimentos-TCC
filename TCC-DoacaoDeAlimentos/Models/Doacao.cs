using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TCCDoacaoDeAlimentos.Models
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
        public string StatusDoacao { get; set; } = "Pendente";
        public List<AlimentoDoacao> Alimentos { get; set; } = new();
        public string Observacao { get; set; }  

    }
}
