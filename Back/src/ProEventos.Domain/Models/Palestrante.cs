using System.ComponentModel.DataAnnotations.Schema;
using ProEventos.Domain.Dtos;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain
{
    [Table("palestrantes")]
    public class Palestrante
    {
        public int Id { get; set; }
        public string MiniCurriculo { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<RedeSocial> RedeSociais { get; set; }
        public IEnumerable<PalestranteEvento> PalestrantesEventos { get; set; }
    }
}