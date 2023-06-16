using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService: IRepositoryEvento
    {
        Task<Evento> AddEvento(Evento evento);
        Task<Evento> UpdateEvento(int eventoId, Evento evento);
        Task<bool> DeleteEventoById(int eventoId);
        Task<bool> DeleteEvento(Evento evento);
    }
}