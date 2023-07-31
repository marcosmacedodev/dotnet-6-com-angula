using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public string MiniCurriculo { get; set; }
        public int UserId { get; set; }
        public UserUpdateDto User { get; set; }
        public IEnumerable<RedeSocialDto> RedeSociais { get; set; }
        public IEnumerable<EventoDto> Eventos { get; set; }
    }
}