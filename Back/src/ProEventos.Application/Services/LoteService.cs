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
        public LoteService(IRepository repository, IRepositoryLote repositoryLote){
            _repositoryLote = repositoryLote;
            _repository = repository;
        }
        private LoteDto ToLoteDto(Lote entity){
            if(entity == null) return null;
            return new LoteDto(){
                Id = entity.Id,
                Nome = entity.Nome,
                Preco = entity.Preco,
                DataInicio = entity.DataInicio.ToString(),
                DataFim = entity.DataFim.ToString(),
                Quantidade = entity.Quantidade,
                EventoId = entity.EventoId
            };
        }
        private Lote ToLote(LoteDto entity){
            if(entity == null) return null;
            DateTime dataInicio, dataFim;
            return new Lote(){
                Id = entity.Id,
                Nome = entity.Nome,
                Preco = entity.Preco,
                DataInicio = DateTime.TryParse(entity.DataInicio, out dataInicio)? dataInicio: null,
                DataFim = DateTime.TryParse(entity.DataFim, out dataFim)? dataFim: null,
                Quantidade = entity.Quantidade,
                EventoId = entity.EventoId
            };
        }
        // private async Task AddLote(int eventoId, Lote entity){
        //     try
        //     {
        //         entity.EventoId = eventoId;
        //         _repository.Add<Lote>(entity);
        //         await _repository.SaveChangesAsync();
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception(ex.Message);
        //     }
        // }
        public async Task<Lote[]> SaveLotes(int eventoId, Lote[] entities)
        {
            try
            {
                Lote[] lotes = await GetLotesByEventoIdAsync(eventoId);

                foreach (Lote entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        await AddLote(eventoId, entity);
                    }
                    else
                    {
                        Lote lote = lotes.FirstOrDefault(lote => lote.Id.Equals(entity.Id))!;
                        //entity.EventoId = eventoId;
                        lote.Nome = entity.Nome;
                        lote.Preco = entity.Preco;
                        lote.Quantidade = entity.Quantidade;
                        lote.DataInicio = entity.DataInicio;
                        lote.DataFim = entity.DataFim;
                        _repository.Update<Lote>(lote);
                        await _repository.SaveChangesAsync();
                    }
                }
                lotes = await GetLotesByEventoIdAsync(eventoId);
                return lotes;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] entities)
        {
            return (await SaveLotes(eventoId, entities.Select(loteDto => ToLote(loteDto)).ToArray())).Select(lote => ToLoteDto(lote)).ToArray();
        }
        public async Task<bool> DeleteLote(int eventoId, int id)
        {
            try
            {
                Lote lote = await GetLoteByIdsAsync(eventoId, id);
                if (lote == null) throw new Exception($"Lote {id} do Evento {eventoId} não encontrado!");
                _repository.Delete<Lote>(lote);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                return await _repositoryLote.GetLotesByEventoIdAsync(eventoId, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int id)
        {
            try
            {
                return await _repositoryLote.GetLoteByIdsAsync(eventoId, id, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> GetLotesDtoByEventoIdAsync(int eventoId)
        {
            try
            {
                return (await GetLotesByEventoIdAsync(eventoId)).Select(lote => ToLoteDto(lote)).ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto> GetLoteDtoByIdsAsync(int eventoId, int id)
        {
            try
            {
                return ToLoteDto(await GetLoteByIdsAsync(eventoId, id));
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public async Task AddLote(int eventoId, Lote entity)
        {
            try
            {
               entity.EventoId = eventoId;
               //entity.Evento = evento;
               _repository.Add<Lote>(entity);
               await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddLote(int eventoId, LoteDto entity)
        {
            await AddLote(eventoId, ToLote(entity));
        }

        public async Task UpdateLote(int eventoId, int loteId, Lote entity)
        {
            try
            {
                //Evento evento = await _repositoryEvento.GetEventoByIdAsync(eventoId, false);
                //if(evento == null) throw new Exception($"Evento {eventoId} não encontrado");

                //Lote lote = evento.Lotes.Where(lote => lote.Id.Equals(loteId)).FirstOrDefault();
                //if (lote == null) throw new Exception($"Lote {loteId} não encontrado");
                Lote lote = await GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) throw new Exception($"Lote {loteId} não encontrado");
                lote.Nome = entity.Nome;
                lote.Preco = entity.Preco;
                lote.Quantidade = entity.Quantidade;
                lote.DataInicio = entity.DataInicio;
                lote.DataFim = entity.DataFim;

                _repository.Update<Lote>(lote);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLote(int eventoId, int loteId, LoteDto entity)
        {
            await UpdateLote(eventoId, loteId, ToLote(entity));
        }
    }
}