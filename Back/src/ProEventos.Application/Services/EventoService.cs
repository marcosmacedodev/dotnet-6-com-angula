using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryEvento _repositoryEvento;
        public EventoService(IRepository repository, IRepositoryEvento repositoryEvento){
            _repositoryEvento = repositoryEvento;
            _repository = repository;

        }
        public async Task<Evento> AddEvento(Evento evento)
        {
            try
            {
                _repository.Add<Evento>(evento);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(evento.Id, false);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(Evento evento)
        {
            return await DeleteEventoById(evento.Id);
        }

        public async Task<bool> DeleteEventoById(int eventoId)
        {
            try
            {
                 Evento evento = await GetEventoByIdAsync(eventoId, false);
                _repository.Delete<Evento>(evento);
                return (await _repository.SaveChangesAsync());
            }
            catch(System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento eventoUpdate)
        {
            try
            {
                Evento evento = await GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;
                eventoUpdate.Id = eventoId;
                _repository.Update<Evento>(eventoUpdate);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(eventoId, false);
                }
                return null;
            }
            catch(System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                 return await _repositoryEvento.GetAllEventosAsync(includePalestrantes);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            try
            {
                return await _repositoryEvento.GetAllEventosByTemaAsync(tema, includePalestrantes);
            }
            catch(System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes)
        {
            try
            {
                Evento evento = await _repositoryEvento.GetEventoByIdAsync(id, includePalestrantes);
                if (evento == null) return null;//throw new Exception($"Evento id: {id}, não encontrado");
                return evento;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}