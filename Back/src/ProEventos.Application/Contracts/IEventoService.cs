using ProEventos.Domain;
using ProEventos.Domain.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<Evento> AddEvento(int userId, Evento entity);
        Task<EventoDto> AddEvento(int userId, EventoDto entity);
        Task<Evento> UpdateEvento(int userId, int eventoId, Evento entity);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto entity);
        Task<bool> DeleteEvento(Evento entity);
        Task<Evento []> GetAllEventosByTemaAsync(int userId, string tema);
        Task<Evento []> GetAllEventosAsync(int userId);
        Task<Evento> GetEventoByIdAsync(int userId, int id);
        Task<EventoDto []> GetAllEventosDtoByTemaAsync(int userId, string tema);
        Task<EventoDto []> GetAllEventosDtoAsync(int userId);
        Task<EventoDto> GetEventoDtoByIdAsync(int userId, int id);
    }
}