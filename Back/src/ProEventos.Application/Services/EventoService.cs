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

        private EventoDto ToEventoDto(Evento evento){
            return new EventoDto()
            {
                Id = evento.Id,
                Local = evento.Local,
                DataEvento = evento.DataEvento.ToString(),
                Tema = evento.Tema,
                QtdPessoas = evento.QtdPessoas,
                ImagemUrl = evento.ImagemUrl,
                Telefone = evento.Telefone,
            };
        }

        private Evento ToEvento(EventoDto evento)
        {
            return new Evento()
            {
                Id = evento.Id,
                Local = evento.Local,
                DataEvento = DateTime.Parse(evento.DataEvento),
                Tema = evento.Tema,
                QtdPessoas = evento.QtdPessoas,
                ImagemUrl = evento.ImagemUrl,
                Telefone = evento.Telefone,
            };
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> AddEvento(EventoDto eventoDto)
        {
            Evento evento = ToEvento(eventoDto);
            evento = await AddEvento(evento);
            return ToEventoDto(evento);
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento evento)
        {
            try
            {
                Evento e = await GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;
                evento.Id = eventoId;
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

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto eventoDto)
        {
            Evento evento = ToEvento(eventoDto);
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

            List<EventoDto> eventosDto = new List<EventoDto>();
            foreach (Evento evento in eventos)
            {
                eventosDto.Add(ToEventoDto(evento));
            }
            return eventosDto.ToArray();
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

            List<EventoDto> eventosDto = new List<EventoDto>();
            
            foreach(Evento evento in eventos)
            {
                eventosDto.Add( ToEventoDto(evento));
            }
            return eventosDto.ToArray();
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes)
        {
            try
            {
                Evento evento = await _repositoryEvento.GetEventoByIdAsync(id, includePalestrantes);
                if (evento == null) return null;//throw new Exception($"Evento id: {id}, não encontrado");
                return evento;
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