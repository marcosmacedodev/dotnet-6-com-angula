using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain.Dtos
{
    public class PalestranteAddDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MiniCurriculo { get; set; }
    }
}