
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
                User entity = _mapper.Map<User>(userUpdateDto);
                if(userUpdateDto.Password != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(entity);
                    await _userManager.ResetPasswordAsync(entity, token, userUpdateDto.Password);
                }
                _repository.Update<User>(entity);
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