
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
    }
}