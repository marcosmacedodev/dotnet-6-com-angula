using System.ComponentModel.DataAnnotations.Schema;
using ProEventos.Domain.Dtos;

namespace ProEventos.Domain
{
    [Table("redessociais")]
    public class RedeSocial
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string URL { get; set; }
        public int? EventoId {get;set;}
        public Evento Evento { get; set; }
        public int? PalestranteId { get; set; }
        public Palestrante Palestrante { get; set; }
    }
}