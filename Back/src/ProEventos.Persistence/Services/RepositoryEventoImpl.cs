using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Services
{
    public class RepositoryEventoImpl : IRepositoryEvento
    {
        private readonly ProEventosContext _context;
        public RepositoryEventoImpl(ProEventosContext context){
            _context = context;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking()
            .OrderBy(e => e.Id);

            if (includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking()
            .OrderBy(e => e.Id);

            if (includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            return await query.Where(e => e.Tema.ToLower().Contains(tema.ToLower())).ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes)
        {
             IQueryable<Evento> query = _context.Eventos
            .Where(e => e.Id.Equals(id))
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