using ProEventos.Domain.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}