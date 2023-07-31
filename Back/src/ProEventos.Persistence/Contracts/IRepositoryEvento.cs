using ProEventos.Domain;
using ProEventos.Persistence.Pages;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryEvento
    {
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes);
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}