using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Pages;

namespace ProEventos.Application.Contracts
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto entity);
        Task<PalestranteDto> UpdatePalestrante(int userId, int PalestranteId, PalestranteUpdateDto entity);
        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos);
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos);
    }
}