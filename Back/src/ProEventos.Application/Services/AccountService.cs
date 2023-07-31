
using AutoMapper;
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
        private readonly IMapper _mapper;
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IRepository repository, IRepositoryUser repositoryUser, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _repositoryUser = repositoryUser;
            _repository = repository;
        }

        private void validateUpdate(ref User user, UserUpdateDto userUpdate)
        {
            UserGrade userGrade;
            UserType userType;
            if(userUpdate.FirstName != null) user.FirstName = userUpdate.FirstName;
            if(userUpdate.LastName != null) user.LastName = userUpdate.LastName;
            if(userUpdate.Email != null) user.Email = userUpdate.Email;
            if(userUpdate.Description != null) user.Description = userUpdate.Description;
            if(userUpdate.ImageUrl != null) user.ImageUrl = userUpdate.ImageUrl;
            if(userUpdate.PhoneNumber != null) user.PhoneNumber = userUpdate.PhoneNumber;
            if(userUpdate.UserGrade != null) user.UserGrade = Enum.TryParse<UserGrade>(userUpdate.UserGrade, true, out userGrade)? userGrade: UserGrade.NaoInformado;
            if(userUpdate.UserType != null) user.UserType = Enum.TryParse<UserType>(userUpdate.UserGrade, true, out userType)? userType: UserType.NaoInformado;
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
                User entity = _mapper.Map<User>(userDto);
                IdentityResult result = await _userManager.CreateAsync(entity, userDto.Password);
                if(result.Succeeded)
                {
                    entity = await _repositoryUser.GetUserByUserNameAsync(userDto.UserName);
                    return _mapper.Map<UserDto>(entity);
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
                User entity = await _repositoryUser.GetUserByUserNameAsync(username);
                return _mapper.Map<UserUpdateDto>(entity);
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
                User entity = await _repositoryUser.GetUserByUserNameAsync(userUpdateDto.UserName);
                validateUpdate(ref entity, userUpdateDto);
                if(userUpdateDto.Password != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(entity);
                    await _userManager.ResetPasswordAsync(entity, token, userUpdateDto.Password);
                }
                _repository.Update<User>(entity);
                if(await _repository.SaveChangesAsync())
                {
                    entity = await _repositoryUser.GetUserByUserNameAsync(userUpdateDto.UserName);
                    return _mapper.Map<UserUpdateDto>(entity);
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