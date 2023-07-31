using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class RedeSocialDto
    {
        public int Id { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public string Nome { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),]
        public string URL { get; set; }
        public int? EventoId {get;set;}
        public EventoDto Evento { get; set; }
        public int? PalestranteId { get; set; }
        public PalestranteDto Palestrante { get; set; }
    }
}