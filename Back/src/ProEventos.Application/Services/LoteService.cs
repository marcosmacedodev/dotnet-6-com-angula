using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryLote _repositoryLote;
        private readonly IMapper _mapper;
        public LoteService(IRepository repository, IRepositoryLote repositoryLote, IMapper mapper){
            _mapper = mapper;
            _repositoryLote = repositoryLote;
            _repository = repository;
        }
        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] lotesDto)
        {
            try
            {
                Lote[] entities = await _repositoryLote.GetLotesByEventoIdAsync(eventoId);

                foreach (LoteDto loteDto in lotesDto)
                {
                    loteDto.EventoId = eventoId;
                    Lote entity = entities.FirstOrDefault(lote => lote.Id.Equals(loteDto.Id));

                    if (entity == null)
                    {
                        await AddLote(eventoId, loteDto);
                    }
                    else
                    {
                        await UpdateLote(eventoId, entity.Id, loteDto);
                    }
                }
                entities = await _repositoryLote.GetLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(entities);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteLote(LoteDto loteDto)
        {
            try
            {
                Lote entity = _mapper.Map<Lote>(loteDto);
                _repository.Delete<Lote>(entity);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                Lote[] entities = await _repositoryLote.GetLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                Lote entity = await _repositoryLote.GetLoteByIdsAsync(eventoId, loteId);
                return _mapper.Map<LoteDto>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task AddLote(int eventoId, LoteDto loteDto)
        {
            try
            {
                Lote entity = _mapper.Map<Lote>(loteDto);
                entity.Id = 0;
                entity.EventoId = eventoId;
                _repository.Add<Lote>(entity);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLote(int eventoId, int loteId, LoteDto loteDto)
        {
            try
            {
                Lote entity = _mapper.Map<Lote>(loteDto);
                entity.EventoId = eventoId;
                entity.Id = loteId;
                _repository.Update<Lote>(entity);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}