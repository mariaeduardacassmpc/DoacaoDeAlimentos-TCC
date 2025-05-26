using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class EntidadeDto
    {
        public string RazaoSocial { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
        public string CNPJ_CPF { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string? Responsavel { get; set; }
    }
}
