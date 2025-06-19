using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using TCCDoacaoDeAlimentos.Shared.Services;

namespace TCCDoacaoDeAlimentos.Shared.Models;

public class Entidade : IValidatableObject
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Selecione o tipo de entidade.")]
    public string TpEntidade { get; set; } = string.Empty;
    public TipoPessoa? TpPessoa { get; set; }
    [Required(ErrorMessage = "Nome Fantasia é obrigatório.")]

    public string NomeFantasia { get; set; }

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
    [RegularExpression(@"^(\(?\d{2}\)?\s?)?\d{4,5}-?\d{4}$", ErrorMessage = "Telefone inválido")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public string Endereco { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP inválido")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "A Cidade é obrigatório.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O E-mail é obrigatório.")]
    public string Email { get; set; }
    public string? Sexo { get; set; }
    public string? Responsavel { get; set; }
    [Required(ErrorMessage = "O número é obrigatório.")]
    public string Numero { get; set; }
    public bool Ativo { get; set; }
    public enum TipoPessoa
    {
        F, // Física
        J  // Jurídica
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var documentoLimpo = Regex.Replace(CNPJ_CPF ?? string.Empty, "[^0-9]", "");

        if (string.IsNullOrEmpty(documentoLimpo))
        {
            if (TpEntidade == "D" && TpPessoa == TipoPessoa.F)
            {
                yield return new ValidationResult("CPF é obrigatório para doador pessoa física.", new[] { nameof(CNPJ_CPF) });
            }
            else if (TpEntidade == "I" || (TpEntidade == "D" && TpPessoa == TipoPessoa.J))
            {
                yield return new ValidationResult("CNPJ é obrigatório para ONG ou doador pessoa jurídica.", new[] { nameof(CNPJ_CPF) });
            }
        }
        else if (TpEntidade == "D" && TpPessoa == TipoPessoa.F)
        {
            if (!DocumentoValidator.ValidarCpf(documentoLimpo))
            {
                yield return new ValidationResult("CPF inválido.", new[] { nameof(CNPJ_CPF) });
            }
        }
        else if (TpEntidade == "I" || (TpEntidade == "D" && TpPessoa == TipoPessoa.J))
        {
            if (!DocumentoValidator.ValidarCnpj(documentoLimpo))
            {
                yield return new ValidationResult("CNPJ inválido.", new[] { nameof(CNPJ_CPF) });
            }
        }
    }
}

