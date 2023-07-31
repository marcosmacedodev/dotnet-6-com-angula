using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.Domain.Identity;

namespace ProEventos.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public readonly SymmetricSecurityKey _key;
        private readonly IMapper _mapper;

        public TokenService(IConfiguration configuration, UserManager<User> userManager, IMapper mapper){
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }
        
        public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
        {
            User user = _mapper.Map<User>(userUpdateDto);
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