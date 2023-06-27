using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Services
{
    public class RepositoryPalestranteImpl: IRepositoryPalestrante
    {
        private readonly ProEventosContext _context;
        public RepositoryPalestranteImpl(ProEventosContext context) {
            _context = context;
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedeSociais);
            if (includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            return await query.OrderBy(p => p.Id).ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllEventosByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedeSociais)
            .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));
            if (includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            return await query.OrderBy(p => p.Id).ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedeSociais)
            .Where(p => p.Id.Equals(id));
            if( includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            return await query.FirstAsync();
        }
    }
}