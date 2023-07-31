using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class PalestranteUpdateDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public string MiniCurriculo { get; set; }
    }
}