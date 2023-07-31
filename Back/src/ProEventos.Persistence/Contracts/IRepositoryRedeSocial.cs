using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRepositoryRedeSocial
    {
        Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}