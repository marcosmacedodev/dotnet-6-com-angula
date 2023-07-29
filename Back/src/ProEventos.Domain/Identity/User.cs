
using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Dtos;
using ProEventos.Domain.Enums;

namespace ProEventos.Domain.Identity
{
    public class User: IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserGrade UserGrade { get; set; }
        public string Description { get; set; }
        public UserType UserType { get; set; }
        public string ImageUrl { get; set; }
        public List<UserRole> UserRoles { get; set; }

        public static explicit operator User(UserDto user){
            return new User(){
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        public static explicit operator User(UserUpdateDto user){
            if (user == null) return null;
            UserGrade userGrade;
            UserType userType;
            return new User(){
                Id = user.Id,
                UserGrade = Enum.TryParse<UserGrade>(user.UserGrade, true, out userGrade)? userGrade: UserGrade.NaoInformado,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserType = Enum.TryParse<UserType>(user.UserType, true, out userType) ? userType: UserType.NaoInformado,
                Description = user.Description
            };
        }
    }
}