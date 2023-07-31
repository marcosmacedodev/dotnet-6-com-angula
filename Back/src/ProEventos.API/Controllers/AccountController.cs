
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Utils;
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
        private readonly IUtilService _utilService;

        private readonly string _folder = "Images";
        public AccountController(IAccountService accountService, ITokenService tokenService, IUtilService utilService)
        {
            _utilService = utilService;
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
            userUpdateDto.Id = User.GetUserId();
            userUpdateDto = await _accountService.UpdateAccountAsync(userUpdateDto);
            if(userUpdateDto == null) return BadRequest($"Erro ao atualizar conta '{userUpdateDto.UserName}'.");
            return Ok(new {
                FirstName = userUpdateDto.FirstName,
                LastName = userUpdateDto.LastName,
                UserName = userUpdateDto.UserName,
                Token = _tokenService.CreateToken(userUpdateDto).Result
            });
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file){
            
            UserUpdateDto userUpdateDto = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            if (userUpdateDto == null) return Unauthorized();
            if (file.Length > 0)
            {
                 _utilService.DeleteImage(userUpdateDto.ImageUrl, "images");
                 string path = await _utilService.SaveImage(file, "images");
                 string fullPath =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/images/{path}";
                 userUpdateDto.ImageUrl = path;
                 await _accountService.UpdateAccountAsync(userUpdateDto);
                 return Created(fullPath, NoContent());
            }
            return BadRequest($"Erro ao tentar fazer upload de imagem para account Id: {userUpdateDto.Id}");
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetImage()
        {
            string userName = User.GetUserName();
            UserUpdateDto userUpdateDto = await _accountService.GetUserByUserNameAsync(userName);
            if (userUpdateDto == null) return Unauthorized();
            string fileName = userUpdateDto.ImageUrl;
            string fullPath =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/{_folder}/{fileName}";
            return Ok(new {
                Protocol = Request.Protocol?.ToLower(),
                IsHttps = Request.IsHttps,
                Host = Request.Host.Value?.ToLower(),
                Port = Request.Host.Port,
                Directory = @$"resources/{_folder}"?.ToLower(),
                FileName = fileName?.ToLower(), 
                FullPath = fullPath?.ToLower()
            });
            
            //return BadRequest($"Erro ao tentar fazer upload de imagem para account Id: {userUpdateDto.Id}");
        }
    }

}