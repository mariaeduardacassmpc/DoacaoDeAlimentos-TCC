using System.ComponentModel.DataAnnotations;

namespace TCCDoacaoDeAlimentos.Shared
{
    public class Alimento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do alimento é obrigatório")]
        [StringLength(150, ErrorMessage = "O nome não pode exceder 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public string Categoria { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
        public string? Descricao { get; set; } = string.Empty;
    }
}
