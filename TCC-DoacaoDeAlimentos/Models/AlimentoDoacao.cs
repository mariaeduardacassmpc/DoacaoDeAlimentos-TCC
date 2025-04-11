
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TCCDoacaoDeAlimentos.Models
{
    public class AlimentoDoacao
    {
        [Key]
        public int Id { get; set; }
        public int IdDoacao { get; set; }

        [Required(ErrorMessage = "O campo Alimento é obrigatório.")]
        [ForeignKey("Alimento")]
        public int AlimentoId { get; set; }

        [Required(ErrorMessage = "A data de validade é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime Validade { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public double Quantidade { get; set; }

        [Required(ErrorMessage = "A unidade de medida é obrigatória.")]
        [StringLength(20, ErrorMessage = "A unidade de medida deve ter no máximo 20 caracteres.")]
        public string UnidadeMedida { get; set; }
    }
}
