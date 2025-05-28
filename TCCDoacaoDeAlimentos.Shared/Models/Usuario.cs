using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCCDoacaoDeAlimentos.Shared.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public int EntidadeId { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        [MaxLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        [MaxLength(20, ErrorMessage = "A senha deve ter no máximo 20 caracteres")]
        public string Senha { get; set; }

        [ForeignKey("EntidadeId")]
        public Entidade Entidade { get; set; }
        public string TipoEntidade { get; set; } 

    }
}