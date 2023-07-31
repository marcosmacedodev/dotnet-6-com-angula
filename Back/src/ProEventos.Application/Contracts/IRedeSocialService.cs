using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto> AddByEvento(int eventoId, RedeSocialDto redeSociaisDto);
        Task<RedeSocialDto> AddByPalestrante(int palestranteId, RedeSocialDto redeSociaisDto);
        Task<RedeSocialDto> UpdateByEvento(int eventoId, int redeSocialId, RedeSocialDto redeSociaisDto);
        Task<RedeSocialDto> UpdateByPalestrante(int palestranteId, int redeSocialId, RedeSocialDto redeSociaisDto);
        Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] redesSociaisDto);
        Task<bool> DeleteByEvento(int eventoId, int redeSocialId);
        Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] redesSociaisDto);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);
        Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);
        Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);
    }
}