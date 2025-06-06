﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FrontDoacaoDeAlimentos.Models
{
    public class Notificacao
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Entidade")]
        public int IdEntidade { get; set; }
        [ForeignKey("Doacao")]
        public int IdDoacao { get; set; }
        public string Mensagem { get; set; }
        public string Observacao { get; set; }
        public string TipoNotificacao { get; set; }
    }
}
