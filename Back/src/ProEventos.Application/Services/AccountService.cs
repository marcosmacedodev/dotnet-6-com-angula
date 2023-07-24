
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.Domain.Enums;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryUser _repositoryUser;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IRepository repository, IRepositoryUser repositoryUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repositoryUser = repositoryUser;
            _repository = repository;
        }

        private User toUser(UserDto userDto){
            if (userDto == null) return null;
            return new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                UserName = userDto.UserName,
            };
        }

        private UserDto toUserDto(User user){
            if (user == null) return null;
            return new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
            };
        }

        private UserUpdateDto toUserUpdateDto(User user){
            if (user == null) return null;
            return new UserUpdateDto()
            {
                  Id = user.Id,
                  UserGrade = user.UserGrade.ToString(),
                  UserName = user.UserName,
                  FirstName = user.FirstName,
                  LastName = user.LastName,
                  Email = user.Email,
                  PhoneNumber = user.PhoneNumber,
                  UserType = user.UserType.ToString(),
                  Description = user.Description,
            };
        }

        private void validateUpdateDto(ref User user, UserUpdateDto userUpdateDto){
            UserGrade userGrade;
            UserType userType;
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
        }


        private User toUser(UserUpdateDto userUpdateDto){
            if(userUpdateDto == null) return null;

            UserGrade userGrade;
            UserType userType;
            return new User()
            {
                  Id = userUpdateDto.Id,
                  UserGrade = Enum.TryParse<UserGrade>(userUpdateDto.UserGrade,true, out userGrade) ? userGrade: UserGrade.NaoInformado,
                  UserName = userUpdateDto.UserName,
                  FirstName = userUpdateDto.FirstName,
                  LastName = userUpdateDto.LastName,
                  Email = userUpdateDto.Email,
                  PhoneNumber = userUpdateDto.PhoneNumber,
                  UserType = Enum.TryParse<UserType>(userUpdateDto.UserType, true, out userType) ? userType: UserType.NaoInformado,
                  Description = userUpdateDto.Description,
            };
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                User user = _userManager.Users.SingleOrDefault(user => user.UserName.Equals(userUpdateDto.UserName.ToLower()));
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<UserDto> CreateUserAccountAsync(UserDto userDto)
        {
            try
            {
                User user = toUser(userDto);
                IdentityResult r = await _userManager.CreateAsync(user, userDto.Password);
                if(r.Succeeded)
                {
                    return toUserDto(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string username)
        {
            try
            {
                User user = await _repositoryUser.GetUserByUserNameAsync(username);
                return toUserUpdateDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                User user = await _repositoryUser.GetUserByUserNameAsync(userUpdateDto.UserName);
                validateUpdateDto(ref user, userUpdateDto);
                if(userUpdateDto.Password != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }
                _repository.Update<User>(user);
                if(await _repository.SaveChangesAsync())
                {
                    return await GetUserByUserNameAsync(userUpdateDto.UserName);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _repositoryUser.GetUserByUserNameAsync(username) != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}