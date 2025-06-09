using System.ComponentModel.DataAnnotations;

namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class AlterarSenhaDto
    {
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).+$", ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula e um caractere especial.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "A confirmação é obrigatória")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarSenha { get; set; }
    }
}
