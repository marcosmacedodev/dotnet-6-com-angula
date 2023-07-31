using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Pages;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto entity);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto entity);
        Task<bool> DeleteEvento(EventoDto entity);
        Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams);
        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId);
    }
}