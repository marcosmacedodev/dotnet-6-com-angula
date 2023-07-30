using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class UserDto
    {
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(4, ErrorMessage = Messages.MINLENGTH),
        MaxLength(64, ErrorMessage = Messages.MAXLENGTH)]
        public string UserName { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        EmailAddress(ErrorMessage = Messages.EMAILADDRESS)]
        public string Email { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(8, ErrorMessage = Messages.MINLENGTH),
        MaxLength(32, ErrorMessage = Messages.MAXLENGTH),
        DataType(DataType.Password)]
        public string Password { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(3, ErrorMessage = Messages.MINLENGTH),
        MaxLength(16, ErrorMessage = Messages.MAXLENGTH)]
        public string FirstName { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(3, ErrorMessage = Messages.MINLENGTH),
        MaxLength(16, ErrorMessage = Messages.MAXLENGTH)]
        public string LastName { get; set; }
    }
}