using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class EventoDto
    {
        public int Id {get;set;}

        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(3, ErrorMessage = Messages.MINLENGTH),
        MaxLength(32, ErrorMessage = Messages.MAXLENGTH)]
        public string Local {get;set;}
        public string DataEvento {get;set;}
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(4, ErrorMessage = Messages.MINLENGTH),
        MaxLength(50, ErrorMessage = Messages.MAXLENGTH)]
        public string Tema {get;set;}
        [Range(1, 120000, ErrorMessage = Messages.RANGE)]
        public int QtdPessoas {get;set;}
        //[RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = Messages.REIMGFORMAT)]
        public string ImagemUrl {get;set;}
        [Required(ErrorMessage = Messages.REQUIRED),
        EmailAddress( ErrorMessage = Messages.EMAILADDRESS)]
        public string Email {get; set;}
        [Required(ErrorMessage = Messages.REQUIRED),
        Phone(ErrorMessage = Messages.PHONE)]
        public string Telefone {get;set;}
        //public IEnumerable<LoteDto>? Lotes {get;set;}
        //public IEnumerable<RedeSocialDto>? RedesSociais {get; set;}
        //public IEnumerable<PalestranteDto>? Palestrantes { get; set; }
    }
}