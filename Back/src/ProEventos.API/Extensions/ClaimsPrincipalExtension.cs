using System.Security.Claims;

namespace ProEventos.API.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserName(this ClaimsPrincipal user){
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user){
            int id;
            string idstr;
            idstr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idstr, out id)? id: 0;
        }
        
    }
}