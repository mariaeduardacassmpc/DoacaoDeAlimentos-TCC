using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCCDoacaoDeAlimentos.Models
{
    public class Ong
    {
        [Key]
        public int IdOng { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "A razão social é obrigatória.")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$", ErrorMessage = "CNPJ no formato inválido (00.000.000/0000-00).")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\) \d{5}\-\d{4}$", ErrorMessage = "Telefone no formato inválido ((00) 00000-0000).")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(150, ErrorMessage = "Máximo de 150 caracteres.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [RegularExpression(@"^\d{5}\-\d{3}$", ErrorMessage = "CEP no formato inválido (00000-000).")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória.")]
        [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O nome do responsável é obrigatório.")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string Responsavel { get; set; }
    }
}
