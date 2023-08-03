using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Pages;

namespace ProEventos.Persistence.Services
{
    public class RepositoryPalestranteImpl: IRepositoryPalestrante
    {
        private readonly ProEventosContext _context;
        public RepositoryPalestranteImpl(ProEventosContext context) {
            _context = context;
        }
        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.User)
            .Include(p => p.RedeSociais)
            .Where(p => (
                p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                p.User.FirstName.ToLower().Contains(pageParams.Term.ToLower()) ||
                p.User.LastName.ToLower().Contains(pageParams.Term.ToLower())) &&
                p.User.UserType == Domain.Enums.UserType.Palestrante 
            )
            .OrderBy(p => p.Id);
            if (includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            
            return await PageList<Palestrante>.CreatePageAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.User)
            .Include(p => p.RedeSociais)
            .Where(p => p.UserId.Equals(userId))
            .AsNoTracking();
            if( includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}