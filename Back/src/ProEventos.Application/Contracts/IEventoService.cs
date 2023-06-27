using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<Evento> AddEvento(Evento evento);
        Task<EventoDto> AddEvento(EventoDto evento);
        Task<Evento> UpdateEvento(int eventoId, Evento evento);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto evento);
        Task<bool> DeleteEventoById(int eventoId);
        Task<bool> DeleteEvento(Evento evento);
        Task<Evento []> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento []> GetAllEventosAsync(bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes);
        Task<EventoDto []> GetAllEventosByTemaAsync(string tema);
        Task<EventoDto []> GetAllEventosAsync();
        Task<EventoDto> GetEventoByIdAsync(int id);
    }
}