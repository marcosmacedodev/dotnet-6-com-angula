using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public string Nome { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public decimal Preco { get; set; }
        [DataType(DataType.DateTime)]
        public string DataInicio { get; set; }
        [DataType(DataType.DateTime)]
        public string DataFim { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED)]
        public int Quantidade { get; set; }
        public int? EventoId { get; set; }
        //public EventoDto Evento {get;set;}
    }
}