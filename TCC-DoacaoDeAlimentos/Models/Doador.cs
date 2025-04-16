using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TCCDoacaoDeAlimentos.Models
{
    public class Doador
    {
        [Key]
        public int IdDoador { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(4, ErrorMessage = "O CPF deve ter no máximo * caracteres.")]
        public string CPF_CNPJ { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(150)]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(60)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(9, ErrorMessage = "O CEP deve estar no formato 00000-000")]
        public string CEP { get; set; }
        public string Cidade { get; set; }


        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        public bool Sexo { get; set; }
        public bool TipoPessoa { get; set; }


    }
}
