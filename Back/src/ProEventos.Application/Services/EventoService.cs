using AutoMapper;
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
        private readonly IMapper _mapper;
        public EventoService(IRepository repository, IRepositoryEvento repositoryEvento, IMapper mapper){
            _mapper = mapper;
            _repositoryEvento = repositoryEvento;
            _repository = repository;
        }

        public async Task<EventoDto> AddEvento(int userId, EventoDto eventoDto)
        {
            try
            {
                Evento entity = _mapper.Map<Evento>(eventoDto);
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
        public async Task<bool> DeleteEvento(EventoDto eventoDto)
        {
            try
            {
                Evento entity = _mapper.Map<Evento>(eventoDto);
                _repository.Delete<Evento>(entity);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto eventoDto)
        {
            try
            {
                Evento entity = _mapper.Map<Evento>(eventoDto);
                _repository.Update<Evento>(entity);
                if (await _repository.SaveChangesAsync())
                {
                    return await GetEventoByIdAsync(userId, eventoId);
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams)
        {
            try
            {
                PageList<Evento> entities = await _repositoryEvento
                .GetAllEventosAsync(userId, pageParams, false);
                return _mapper.Map<PageList<EventoDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId)
        {
            try
            {
                Evento entity = await _repositoryEvento.GetEventoByIdAsync(userId, eventoId, false);
                return _mapper.Map<EventoDto>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}