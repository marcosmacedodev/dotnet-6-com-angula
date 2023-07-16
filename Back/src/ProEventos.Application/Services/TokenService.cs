using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.Domain.Enums;
using ProEventos.Domain.Identity;

namespace ProEventos.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration, UserManager<User> userManager){
            _userManager = userManager;
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }

        private User ToUser(UserUpdateDto userUpdateDto){
            UserGrade userGrade;
            UserType userType;
            User user = new User();
            if(userUpdateDto.Id > 0){
                user.Id = userUpdateDto.Id;
            }
            if (userUpdateDto.UserName != null){
                user.UserName = userUpdateDto.UserName;
            }
            if(userUpdateDto.FirstName != null){
                user.FirstName = userUpdateDto.FirstName;
            }
            if(userUpdateDto.LastName != null){
                user.LastName = userUpdateDto.LastName;
            }
            if(userUpdateDto.PhoneNumber != null){
                user.PhoneNumber = userUpdateDto.PhoneNumber;
            }
            if(userUpdateDto.UserGrade != null){
                user.UserGrade = Enum.TryParse<UserGrade>(userUpdateDto.UserGrade,true, out userGrade) ? userGrade: UserGrade.NaoInformado;
            }
            if(userUpdateDto.UserType != null){
                user.UserType = Enum.TryParse<UserType>(userUpdateDto.UserType, true, out userType) ? userType: UserType.NaoInformado;
            }
            if(userUpdateDto.Email != null){
                user.Email = userUpdateDto.Email;
            }
            if(userUpdateDto.Description != null){
                user.Description = userUpdateDto.Description;
            }
            return user;
        }
        
        public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
        {
            User user = ToUser(userUpdateDto);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            IList<string> roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
            securityTokenDescriptor.Subject = new ClaimsIdentity(claims);
            securityTokenDescriptor.Expires = DateTime.Now.AddDays(1);
            securityTokenDescriptor.SigningCredentials = creds;

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = securityTokenHandler.CreateToken(securityTokenDescriptor);
            return securityTokenHandler.WriteToken(token);
        }
    }
}