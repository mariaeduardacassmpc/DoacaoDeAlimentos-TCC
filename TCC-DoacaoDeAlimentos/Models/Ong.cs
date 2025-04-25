using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TCCDoacaoDeAlimentos.Models
{
    public class Ong
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Razão social é obrigatória.")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$",
            ErrorMessage = "Formato: 00.000.000/0000-00.")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\) \d{4,5}\-\d{4}$",
            ErrorMessage = "Formato: (00) 0000-0000 ou (00) 00000-0000.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        [StringLength(150, ErrorMessage = "Máximo de 150 caracteres.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Bairro é obrigatório.")]
        [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório.")]
        [RegularExpression(@"^\d{5}\-\d{3}$", ErrorMessage = "Formato: 00000-000.")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória.")]
        [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Responsável é obrigatório.")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string Responsavel { get; set; }
    }
}