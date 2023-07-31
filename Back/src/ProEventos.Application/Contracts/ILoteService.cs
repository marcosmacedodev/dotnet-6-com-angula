using ProEventos.Domain;
using ProEventos.Domain.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface ILoteService
    {
        Task AddLote(int eventoId, LoteDto loteDto);
        Task UpdateLote(int eventoId, int loteId, LoteDto loteDto);
        Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] entities);
        Task<bool> DeleteLote(LoteDto loteDto);
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);

    }
}