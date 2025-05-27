using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

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

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter no minímo 8 caracteres")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "Confirme sua senha.")]
    [DataType(DataType.Password)]
    [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmarSenha { get; set; }

    [Required(ErrorMessage = "Documento é obrigatório.")]
    [CustomValidation(typeof(Entidade), "ValidateDocumento")]
    public string CNPJ_CPF { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório.")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(150, ErrorMessage = "Máximo de 150 caracteres.")]
    public string Endereco { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
    public string Altitude { get; set; }
    public string Latitude;

    [Required(ErrorMessage = "Bairro é obrigatório.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "CEP é obrigatório.")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "Cidade é obrigatória.")]
    [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
    [Required(ErrorMessage =" Obrigatório escolher o sexo")]
    public string Sexo { get; set; }

    [Required(ErrorMessage = "Responsável é obrigatório.")]
    public string Responsavel { get; set; }
    public string Numero { get; set; }

    public static ValidationResult ValidateDocumento(string documento, ValidationContext context)
    {
        var entidade = (Entidade)context.ObjectInstance;

        // Remove máscaras para validação
        var docSemMascara = documento?.Replace(".", "").Replace("-", "").Replace("/", "");

        if (entidade.Tipo == TipoEntidade.F)
        {
            if (docSemMascara?.Length != 11)
                return new ValidationResult("CPF deve ter 11 dígitos");
        }
        else // Para ONG (O) e Pessoa Jurídica (J)
        {
            if (docSemMascara?.Length != 14)
                return new ValidationResult("CNPJ deve ter 14 dígitos");
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
