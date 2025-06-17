using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class OngDto
    {
        public string NomeFantasia { get; set; }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Responsavel { get; set; }
    }
}
