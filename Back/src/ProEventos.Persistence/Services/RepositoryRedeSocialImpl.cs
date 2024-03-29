using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Services
{
    public class RepositoryRedeSocialImpl : IRepositoryRedeSocial
    {
        private readonly ProEventosContext _context;

        public RepositoryRedeSocialImpl(ProEventosContext context)
        {
            _context = context;
        }
        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
            .Include(rs => rs.Evento)
            .Include(rs => rs.Palestrante)
            .Where(rs => rs.EventoId.Equals(eventoId))
            .OrderBy(rs => rs.Id)
            .AsNoTracking();
            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
            .Include(rs => rs.Evento)
            .Include(rs => rs.Palestrante)
            .Where(rs => rs.PalestranteId.Equals(palestranteId))
            .OrderBy(rs => rs.Id)
            .AsNoTracking();
            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
            .Include(rs => rs.Evento)
            .Include(rs => rs.Palestrante)
            .Where(rs => rs.Id.Equals(redeSocialId) && rs.EventoId.Equals(eventoId))
            .AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
            .Include(rs => rs.Evento)
            .Include(rs => rs.Palestrante)
            .Where(rs => rs.Id.Equals(redeSocialId) && rs.PalestranteId.Equals(palestranteId))
            .AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }
    }
}