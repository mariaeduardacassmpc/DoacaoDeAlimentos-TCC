using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCCDoacaoDeAlimentos.Shared.Models;

public class Entidade
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O tipo de pessoa é obrigatório.")]
    public TipoEntidade Tipo { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
    public string RazaoSocial { get; set; }

    [Required(ErrorMessage = "Documento é obrigatório.")]
    [CustomValidation(typeof(Entidade), "ValidateDocumento")]
    public string CNPJ_CPF { get; set; }

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
    public string? Sexo { get; set; }
    public string? Responsavel { get; set; }

    public static ValidationResult ValidateDocumento(string documento, ValidationContext context)
    {
        var entidade = (Entidade)context.ObjectInstance;

        if (entidade.Tipo == TipoEntidade.F)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(documento, @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$"))
                return new ValidationResult("CPF deve estar no formato 000.000.000-00");
        }
        else // Para ONG (O) e Pessoa Jurídica (J) usamos CNPJ
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(documento, @"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$"))
                return new ValidationResult("CNPJ deve estar no formato 00.000.000/0000-00");
        }

        return ValidationResult.Success;
    }

    public enum TipoEntidade
    {
        F, // Física
        J, // Jurídica
        O  // ONG
    }
}
