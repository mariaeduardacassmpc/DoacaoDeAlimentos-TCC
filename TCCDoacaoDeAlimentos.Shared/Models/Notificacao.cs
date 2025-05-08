using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FrontDoacaoDeAlimentos.Models
{
    public class Notificacao
    {
        [Key]
        public int IdNotificacao { get; set; }
        [ForeignKey("Doador")]
        public int IdDoador { get; set; }
        [ForeignKey("Doacao")]
        public int IdDoacao { get; set; }
        public string Mensagem { get; set; }
        public string Observacao { get; set; }
        public string TipoNotificacao { get; set; }
    }
}
