using ProEventos.Domain;
using ProEventos.Domain.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<Evento> AddEvento(Evento entity);
        Task<EventoDto> AddEvento(EventoDto entity);
        Task<Evento> UpdateEvento(int eventoId, Evento entity);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto entity);
        Task<bool> DeleteEventoById(int eventoId);
        Task<bool> DeleteEvento(Evento entity);
        Task<Evento []> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento []> GetAllEventosAsync(bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes);
        Task<EventoDto []> GetAllEventosByTemaAsync(string tema);
        Task<EventoDto []> GetAllEventosAsync();
        Task<EventoDto> GetEventoByIdAsync(int id);
    }
}