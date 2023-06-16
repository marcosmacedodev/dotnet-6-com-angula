using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryEvento
    {
        Task<Evento []> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento []> GetAllEventosAsync(bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes);
    }
}