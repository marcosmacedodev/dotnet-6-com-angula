using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Services
{
    public class RepositoryUserImpl : IRepositoryUser
    {
        private readonly ProEventosContext _context;

        public RepositoryUserImpl(ProEventosContext context)
        {
            _context = context;    
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.UserName == username.ToLower());
        }

        public async Task<User[]> GetUsersAsync()
        {
            return await _context.Users.ToArrayAsync();
        }
    }
}