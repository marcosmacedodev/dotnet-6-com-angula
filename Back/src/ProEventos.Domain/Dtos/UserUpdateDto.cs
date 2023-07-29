using System.ComponentModel.DataAnnotations;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Util;

namespace ProEventos.Domain.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
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
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        //public string Token { get; set; }

        public static explicit operator UserUpdateDto(User user){
            if (user == null) return null;
            return new UserUpdateDto(){
                Id = user.Id,
                UserGrade = user.UserGrade.ToString(),
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType.ToString(),
                Description = user.Description
            };
        }
    }
}