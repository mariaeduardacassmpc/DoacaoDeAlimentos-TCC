using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TCCDoacaoDeAlimentos.Shared.Models
{
    public class AlimentoDoacao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Doacao")]
        public int IdDoacao { get; set; }

        [JsonIgnore]
        public Doacao? Doacao { get; set; }

        [Required(ErrorMessage = "O alimento é obrigatório.")]
        [ForeignKey("Alimento")]
        public int AlimentoId { get; set; }

        [JsonIgnore]
        public Alimento? Alimento { get; set; }

        [Required(ErrorMessage = "A data de validade é obrigatória.")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2025-04-10", "2030-12-31", ErrorMessage = "A data deve estar entre 10/04/2025 e 31/12/2030.")]
        public DateTime Validade { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "A unidade de medida é obrigatória.")]
        [StringLength(20, ErrorMessage = "Máximo de 20 caracteres.")]
        public string UnidadeMedida { get; set; }
    }
}
