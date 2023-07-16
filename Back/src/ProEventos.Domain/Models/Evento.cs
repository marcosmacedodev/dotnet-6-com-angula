using System.ComponentModel.DataAnnotations.Schema;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain
{
    [Table("eventos")]
    public class Evento
    {
        public int Id {get;set;}
        public string Local {get;set;}
        public DateTime? DataEvento {get;set;}
        public string Tema {get;set;}
        public int QtdPessoas {get;set;}
        public string ImagemUrl {get;set;}
        public string Email {get; set;}
        public string Telefone {get;set;}
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Lote> Lotes {get;set;}
        public IEnumerable<RedeSocial> RedesSociais {get; set;}
        public IEnumerable<PalestranteEvento> PalestrantesEventos { get; set; }
    }
}