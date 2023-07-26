using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Pages;

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
        private Evento ToEvento(EventoDto eventoDto)
        {
            if (eventoDto == null) return null;
            DateTime dataEvento;
            return new Evento()
            {
                Id = eventoDto.Id,
                Local = eventoDto.Local,
                DataEvento = DateTime.TryParse(eventoDto.DataEvento, out dataEvento)? dataEvento : null,
                Tema = eventoDto.Tema,
                Email = eventoDto.Email,
                QtdPessoas = eventoDto.QtdPessoas,
                //ImagemUrl = eventoDto.ImagemUrl,
                Telefone = eventoDto.Telefone,
            };
        }

        public void ValidateEvento(ref Evento entity, EventoDto eventoDto){
                DateTime dataEvento;
                if (eventoDto.DataEvento != null) entity.DataEvento = DateTime.TryParse(eventoDto.DataEvento, out dataEvento)? dataEvento : null;
                if (eventoDto.Email != null) entity.Email = eventoDto.Email;
                if (eventoDto.Local != null) entity.Local = eventoDto.Local;
                if (eventoDto.QtdPessoas != 0) entity.QtdPessoas = eventoDto.QtdPessoas;
                if (eventoDto.Telefone != null) entity.Telefone = eventoDto.Telefone;
                if (eventoDto.Tema != null) entity.Tema = eventoDto.Tema;
        }

        public async Task<Evento> AddEvento(int userId, Evento entity)
        {
            try
            {
                entity.UserId = userId;
                entity.ImagemUrl = null;
                _repository.Add<Evento>(entity);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(userId, entity.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> AddEvento(int userId, EventoDto entity)
        {
            Evento evento = ToEvento(entity);
            evento = await AddEvento(userId,evento);
            return ToEventoDto(evento);
        }
        public async Task<bool> DeleteEvento(Evento entity)
        {
            _repository.Delete(entity);
            return await _repository.SaveChangesAsync();
        }

        public async Task<Evento> UpdateEvento(int userId, int eventoId, Evento entity)
        {
            try
            {
                _repository.Update<Evento>(entity);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(userId,eventoId);
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto eventoDto)
        {
            Evento evento = ToEvento(eventoDto);
            evento = await UpdateEvento(userId, eventoId, evento);
            return ToEventoDto(evento);
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams)
        {
            try
            {
                return await _repositoryEvento.GetAllEventosAsync(userId, pageParams, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PageList<EventoDto>> GetAllEventosDtoAsync(int userId, PageParams pageParams)
        {
            PageList<Evento> eventos = await GetAllEventosAsync(userId, pageParams);
            
            return await PageList<EventoDto>.
                                CreatePageAsync(
                                    eventos.Items.Select(evento => ToEventoDto(evento))
                                    .AsQueryable(), pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int id)
        {
            try
            {
                return await _repositoryEvento.GetEventoByIdAsync(userId, id, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoDtoByIdAsync(int userId, int eventoId)
        {
            Evento evento = await GetEventoByIdAsync(userId, eventoId);
            return ToEventoDto(evento);
        }
    }
}