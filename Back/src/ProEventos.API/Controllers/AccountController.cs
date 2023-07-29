
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }
        
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
           string userName = User.GetUserName();
           UserUpdateDto user = await _accountService.GetUserByUserNameAsync(userName);
           if (user == null) return Unauthorized();
           return Ok(user);
        }

        [HttpPost("createuser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            if (await _accountService.UserExists(userDto.UserName))
                return BadRequest("Usuário já está em uso.");
            userDto = await _accountService.CreateUserAccountAsync(userDto);
            return Created("", userDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            UserUpdateDto user = await _accountService.GetUserByUserNameAsync(loginDto.UserName);
            if(user == null) return Unauthorized();
            var signInResult = await _accountService.CheckUserPasswordAsync(user, loginDto.Password);
            if (!signInResult.Succeeded) return Unauthorized();
            return Ok(new {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user).Result
            });
        }

        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto){
            if (!userUpdateDto.UserName.Equals(User.GetUserName()))
                return Unauthorized();
            UserUpdateDto user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            if(user == null) Unauthorized();
            user = await _accountService.UpdateAccountAsync(userUpdateDto);
            if(user == null) return BadRequest($"Erro ao atualizar conta '{userUpdateDto.UserName}'.");
            return Ok(new {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user).Result
            });
        }

    }

}