using ProEventos.Domain;
using ProEventos.Persistence.Pages;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryEvento
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int id, bool includePalestrantes);
    }
}