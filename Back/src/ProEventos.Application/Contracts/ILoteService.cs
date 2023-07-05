
using ProEventos.Domain;
using ProEventos.Domain.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface ILoteService
    {
        Task AddLote(int eventoId, Lote entity);
        Task AddLote(int eventoId, LoteDto entity);
        Task UpdateLote(int eventoId, int loteId, Lote entity);
        Task UpdateLote(int eventoId, int loteId, LoteDto entity);
        Task<Lote[]> SaveLotes(int eventoId, Lote[] entity);
        Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] entities);
        Task<bool> DeleteLote(int eventoId, int id);
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
        Task<LoteDto[]> GetLotesDtoByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteDtoByIdsAsync(int eventoId, int id);

    }
}