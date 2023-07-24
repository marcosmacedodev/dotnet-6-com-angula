using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class LoginDto
    {
        [Required( ErrorMessage = Messages.REQUIRED)]
        public string UserName { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        DataType(DataType.Password)]
        public string Password { get; set; }
    }
}