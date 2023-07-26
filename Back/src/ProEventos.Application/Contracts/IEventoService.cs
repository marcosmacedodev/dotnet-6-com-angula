using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Pages;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<Evento> AddEvento(int userId, Evento entity);
        Task<EventoDto> AddEvento(int userId, EventoDto entity);
        Task<Evento> UpdateEvento(int userId, int eventoId, Evento entity);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto entity);
        Task<bool> DeleteEvento(Evento entity);
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams);
        Task<Evento> GetEventoByIdAsync(int userId, int id);
        Task<PageList<EventoDto>> GetAllEventosDtoAsync(int userId, PageParams pageParams);
        Task<EventoDto> GetEventoDtoByIdAsync(int userId, int id);
    }
}