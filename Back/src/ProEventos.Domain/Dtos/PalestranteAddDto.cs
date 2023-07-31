using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class PalestranteAddDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public string MiniCurriculo { get; set; }
    }
}