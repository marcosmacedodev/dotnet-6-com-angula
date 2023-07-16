using ProEventos.Domain;
using Microsoft.EntityFrameworkCore;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Services
{
    public class RepositoryLoteImpl : IRepositoryLote
    {
        private readonly ProEventosContext _context;
        public RepositoryLoteImpl(ProEventosContext context){
            _context = context;
        }

        public async Task<Lote[]> GetAllLotes(bool includeEvento)
        {
            IQueryable<Lote> query = _context.Lotes
                                    //.Include(lote => lote.Evento)
                                    .AsNoTracking()
                                    .OrderBy(lote => lote.Id);
            if (includeEvento)
            {
                query = query.Include(lote => lote.Evento);
            }
            return await query.ToArrayAsync();
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId, bool includeEvento)
        {
            IQueryable<Lote> query = _context.Lotes
                                    //.Include(lote => lote.Evento)
                                    .AsNoTracking()
                                    .Where(lote => lote.EventoId.Equals(eventoId))
                                    .Where(lote => lote.Id.Equals(loteId)).OrderBy(lote => lote.Id)
                                    .OrderBy(lote => lote.Id);
            if (includeEvento)
            {
                query = query.Include(lote => lote.Evento);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId, bool includeEvento)
        {
            IQueryable<Lote> query = _context.Lotes
                                    //.Include(lote => lote.Evento)
                                    .AsNoTracking()
                                    .Where(lote => lote.EventoId.Equals(eventoId))
                                    .OrderBy(lote => lote.Id);
            if (includeEvento)
            {
                query = query.Include(lote => lote.Evento);
            }

            return await query.ToArrayAsync();
        }
    }
}