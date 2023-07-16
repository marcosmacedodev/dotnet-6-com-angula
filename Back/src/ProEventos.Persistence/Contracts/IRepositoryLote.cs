using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryLote
    {
        Task<Lote[]> GetAllLotes(bool includeEvento);
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId, bool includeEvento);
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId, bool includeEvento);
    }
}