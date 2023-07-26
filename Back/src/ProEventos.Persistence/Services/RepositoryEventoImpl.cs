using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Pages;

namespace ProEventos.Persistence.Services
{
    public class RepositoryEventoImpl : IRepositoryEvento
    {
        private readonly ProEventosContext _context;
        
        public RepositoryEventoImpl(ProEventosContext context){
            _context = context;
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Where(e => e.UserId.Equals(userId))
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking()
            .OrderBy(e => e.Id);

            if (includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            return await query.ToArrayAsync();
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Where(e => e.Tema.ToLower().Contains(pageParams.Term.ToLower()))
            .Where(e => e.UserId.Equals(userId))
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking()
            .OrderBy(e => e.Id);

            if (includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            return await PageList<Evento>.CreatePageAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int id, bool includePalestrantes)
        {
             IQueryable<Evento> query = _context.Eventos
            .Where(e => e.Id.Equals(id))
            .Where(e => e.UserId.Equals(userId))
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking()
            .OrderBy(e => e.Id);

            if (includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}