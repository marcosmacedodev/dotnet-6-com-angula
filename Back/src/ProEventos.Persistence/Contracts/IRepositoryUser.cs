using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryUser
    {
        Task<User []> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string username);
    }
}