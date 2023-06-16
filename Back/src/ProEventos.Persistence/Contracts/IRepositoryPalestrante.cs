using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryPalestrante
    {
        Task<Palestrante []> GetAllEventosByNomeAsync(string nome, bool includeEventos);
        Task<Palestrante []> GetAllPalestrantesAsync(bool includeEventos);
        Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos);
    }
}