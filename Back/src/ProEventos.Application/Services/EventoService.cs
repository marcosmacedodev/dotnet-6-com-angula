using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
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
        private EventoDto ToEventoDto(Evento entity)
        {
            if (entity == null) return null;
            return new EventoDto()
            {
                Id = entity.Id,
                Local = entity.Local,
                DataEvento = entity.DataEvento.ToString(),
                Tema = entity.Tema,
                Email = entity.Email,
                QtdPessoas = entity.QtdPessoas,
                ImagemUrl = entity.ImagemUrl,
                Telefone = entity.Telefone,
            };
        }
        private Evento ToEvento(EventoDto entity)
        {
            if (entity == null) return null;
            DateTime dataEvento;
            return new Evento()
            {
                Id = entity.Id,
                Local = entity.Local,
                DataEvento = DateTime.TryParse(entity.DataEvento, out dataEvento)? dataEvento : null,
                Tema = entity.Tema,
                Email = entity.Email,
                QtdPessoas = entity.QtdPessoas,
                ImagemUrl = entity.ImagemUrl,
                Telefone = entity.Telefone,
            };
        }
        public async Task<Evento> AddEvento(Evento entity)
        {
            try
            {
                _repository.Add<Evento>(entity);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(entity.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> AddEvento(EventoDto entity)
        {
            Evento evento = ToEvento(entity);
            evento = await AddEvento(evento);
            return ToEventoDto(evento);
        }
        public async Task<bool> DeleteEvento(Evento entity)
        {
            return await DeleteEventoById(entity.Id);
        }
        public async Task<bool> DeleteEventoById(int eventoId)
        {
            try
            {
                 Evento evento = await GetEventoByIdAsync(eventoId, false);
                 if (evento == null) throw new Exception($"Evento {eventoId} não foi encontrado");
                _repository.Delete<Evento>(evento);
                return (await _repository.SaveChangesAsync());
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento> UpdateEvento(int eventoId, Evento entity)
        {
            try
            {
                Evento evento = await GetEventoByIdAsync(eventoId, false);
                if (evento == null) throw new Exception($"Evento {eventoId} não foi encontrado");
                evento.DataEvento = entity.DataEvento;
                evento.Email = entity.Email;
                evento.ImagemUrl = entity.ImagemUrl;
                evento.Local = entity.Local;
                evento.QtdPessoas = entity.QtdPessoas;
                evento.Telefone = entity.Telefone;
                evento.Tema = entity.Tema;
                _repository.Update<Evento>(evento);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(eventoId, false);
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto entity)
        {
            Evento evento = ToEvento(entity);
            evento = await UpdateEvento(eventoId, evento);
            return ToEventoDto(evento);
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            try
            {
                return await _repositoryEvento.GetAllEventosAsync(includePalestrantes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto[]> GetAllEventosAsync()
        {
            Evento[] eventos = await GetAllEventosAsync(false);
            return eventos.Select(evento => ToEventoDto(evento)).ToArray();
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            try
            {
                return await _repositoryEvento.GetAllEventosByTemaAsync(tema, includePalestrantes);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema)
        {
            Evento[] eventos = await GetAllEventosByTemaAsync(tema, false);
            
            return eventos.Select(evento => ToEventoDto(evento)).ToArray();
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes)
        {
            try
            {
                return await _repositoryEvento.GetEventoByIdAsync(id, includePalestrantes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int id)
        {
            Evento evento = await GetEventoByIdAsync(id, false);
            return ToEventoDto(evento);
        }
    }
}