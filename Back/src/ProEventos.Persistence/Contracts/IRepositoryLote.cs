using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryLote
    {
        Task<Lote[]> GetAllLotes();
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}