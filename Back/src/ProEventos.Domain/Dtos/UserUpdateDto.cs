using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Enums;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        [EnumDataType(typeof(UserGrade))]
        public string UserGrade { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(4, ErrorMessage = Messages.MINLENGTH),
        MaxLength(64, ErrorMessage = Messages.MAXLENGTH)]
        public string UserName { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(3, ErrorMessage = Messages.MINLENGTH),
        MaxLength(16, ErrorMessage = Messages.MAXLENGTH)]
        public string FirstName { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        MinLength(3, ErrorMessage = Messages.MINLENGTH),
        MaxLength(16, ErrorMessage = Messages.MAXLENGTH)]
        public string LastName { get; set; }
        [Required( ErrorMessage = Messages.REQUIRED),
        EmailAddress(ErrorMessage = Messages.EMAILADDRESS)]
        public string Email { get; set; }
        [Phone(ErrorMessage = Messages.PHONE)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        [EnumDataType(typeof(UserType))]
        public string UserType { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //public string Token { get; set; }
    }
}