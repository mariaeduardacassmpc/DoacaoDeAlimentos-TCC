using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCDoacaoDeAlimentos.Shared.Models
{
    public class DepoimentoModel
    {
        [Required(ErrorMessage = "Por favor, informe seu nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe seu email")]
        [EmailAddress(ErrorMessage = "Por favor, informe um email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor, escreva seu depoimento")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "O depoimento deve ter entre 10 e 500 caracteres")]
        public string Mensagem { get; set; }
    }
}
