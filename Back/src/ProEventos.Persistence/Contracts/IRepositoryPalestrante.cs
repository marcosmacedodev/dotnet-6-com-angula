using ProEventos.Domain;
using ProEventos.Persistence.Pages;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryPalestrante
    {
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos);
        Task<Palestrante> GetPalestranteByIdAsync(int userId, bool includeEventos);
    }
}