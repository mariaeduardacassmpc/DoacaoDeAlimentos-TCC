using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Documento é obrigatório.")]
        [CustomValidation(typeof(Doador), "ValidateDocumento")]
        public string CPF_CNPJ { get; set; }

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

        public bool Sexo { get; set; }
        public bool TipoPessoa { get; set; }

        //validação das mascaras
        public static ValidationResult ValidateDocumento(string documento, ValidationContext context)
        {
            var doador = (Doador)context.ObjectInstance;

            if (doador.TipoPessoa) // Pessoa Física (CPF)
            {
                if (string.IsNullOrEmpty(documento) || !System.Text.RegularExpressions.Regex.IsMatch(documento, @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$"))
                {
                    return new ValidationResult("CPF deve estar no formato 000.000.000-00");
                }

                // Remove a máscara para contar os dígitos
                var digits = documento.Replace(".", "").Replace("-", "");
                if (digits.Length != 11)
                {
                    return new ValidationResult("CPF deve ter 11 dígitos");
                }
            }
            else // Pessoa Jurídica (CNPJ)
            {
                if (string.IsNullOrEmpty(documento) || !System.Text.RegularExpressions.Regex.IsMatch(documento, @"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$"))
                {
                    return new ValidationResult("CNPJ deve estar no formato 00.000.000/0000-00");
                }

                // Remove a máscara para contar os dígitos
                var digits = documento.Replace(".", "").Replace("/", "").Replace("-", "");
                if (digits.Length != 14)
                {
                    return new ValidationResult("CNPJ deve ter 14 dígitos");
                }
            }

            return ValidationResult.Success;
        }
    }
}
