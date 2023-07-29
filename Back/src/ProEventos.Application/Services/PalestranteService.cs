using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Pages;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestranteService
    {
        public PalestranteService(IRepository repository, IRepositoryPalestrante repositoryPalestrante)
        {
            
        }
        public Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(int userId, PageParams pageParams, bool includeEventos)
        {
            throw new NotImplementedException();
        }

        public Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            throw new NotImplementedException();
        }

        public Task<PalestranteDto> UpdatePalestrante(int userId, int PalestranteId, PalestranteUpdateDto entity)
        {
            throw new NotImplementedException();
        }
    }
}