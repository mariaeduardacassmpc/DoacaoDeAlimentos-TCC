using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using TCCDoacaoDeAlimentos.Shared.Services;

namespace TCCDoacaoDeAlimentos.Shared.Models;

public class Entidade : IValidatableObject
{
    [Key]
    public int Id { get; set; }
    public TipoEntidade TpEntidade { get; set; }
    public TipoPessoa TpPessoa { get; set; }
    [Required(ErrorMessage = "Nome é obrigatório.")]

    public string RazaoSocial { get; set; }
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).+$", ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula e um caractere especial.")]

    [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter no minímo 8 caracteres")]

    public string Senha { get; set; }
    [Required(ErrorMessage = "Confirme sua senha.")]
    [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]

    public string ConfirmarSenha { get; set; }
    [Required(ErrorMessage = "Documento é obrigatório.")]
    public string CNPJ_CPF { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório.")]
    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone inválido")]

    public string Telefone { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public string Endereco { get; set; }
    public double? Altitude { get; set; }
    public double? Latitude { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP inválido")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "A Cidade é obrigatório.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O E-mail é obrigatório.")]
    public string Email { get; set; }

    public string? Sexo { get; set; }
    public string? Responsavel { get; set; }

    [Required(ErrorMessage = "O Número é obrigatório.")]
    public string Numero { get; set; }
    public bool Ativo { get; set; }

    public enum TipoEntidade
    {
        D, // Doador
        O  // ONG
    }

    public enum TipoPessoa
    {
        F, // Física
        J  // Jurídica
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var documentoLimpo = Regex.Replace(CNPJ_CPF ?? "", "[^0-9]", "");

        if (TpPessoa == TipoPessoa.F) 
        {
            if (!DocumentoValidator.ValidarCpf(documentoLimpo))
            {
                yield return new ValidationResult("CPF inválido.", new[] { nameof(CNPJ_CPF) });
            }
        }
        else if (TpPessoa == TipoPessoa.J) 
        {
            if (!DocumentoValidator.ValidarCnpj(documentoLimpo))
            {
                yield return new ValidationResult("CNPJ inválido.", new[] { nameof(CNPJ_CPF) });
            }
        }
    }
}


