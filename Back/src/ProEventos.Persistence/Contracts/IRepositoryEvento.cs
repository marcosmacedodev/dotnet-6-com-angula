using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryEvento
    {
        Task<Evento []> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);
        Task<Evento []> GetAllEventosAsync(int userId, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int id, bool includePalestrantes);
    }
}