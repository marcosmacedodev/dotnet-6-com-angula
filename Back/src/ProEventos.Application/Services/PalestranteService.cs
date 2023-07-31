using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Pages;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryPalestrante _repositoryPalestrante;
        private readonly IMapper _mapper;

        public PalestranteService(IRepository repository, IRepositoryPalestrante repositoryPalestrante, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryPalestrante = repositoryPalestrante;
            _repository = repository;
        }
        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto palAddDto)
        {
           try
           {
                Palestrante entity = _mapper.Map<Palestrante>(palAddDto);
                entity.Id = 0;
                entity.UserId = userId;
                _repository.Add<Palestrante>(entity);
                if(await _repository.SaveChangesAsync())
                {
                    entity = await _repositoryPalestrante.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(entity);
                }
                return null;
           }
           catch (Exception ex)
           {
                throw new Exception(ex.Message);
           }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos)
        {
            try
            {
                PageList<Palestrante> entities = await _repositoryPalestrante
                                                    .GetAllPalestrantesAsync(pageParams, includeEventos);
                return _mapper.Map<PageList<PalestranteDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            try
            {
                Palestrante entity = await _repositoryPalestrante
                .GetPalestranteByUserIdAsync(userId, includeEventos);
                return _mapper.Map<PalestranteDto>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, int palestranteId, PalestranteUpdateDto palUpDto)
        {
            try
            {
                Palestrante entity = _mapper.Map<Palestrante>(palUpDto);
                entity.Id = palestranteId;
                entity.UserId = userId;
                _repository.Update<Palestrante>(entity);
                if (await _repository.SaveChangesAsync())
                {
                    entity = await _repositoryPalestrante.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(entity);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}