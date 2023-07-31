using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Services
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryRedeSocial _repositoryRedeSocial;
        private readonly IRepository _repository;
        public RedeSocialService(IRepository repository, IRepositoryRedeSocial repositoryRedeSocial, IMapper mapper)
        {
            _repository = repository;
            _repositoryRedeSocial = repositoryRedeSocial;
            _mapper = mapper;
        }

        public async Task<RedeSocialDto> AddByEvento(int eventoId, RedeSocialDto redeSociaisDto)
        {
            try
            {
                RedeSocial entity = _mapper.Map<RedeSocial>(redeSociaisDto);
                entity.Id = 0;
                entity.PalestranteId = null;
                entity.EventoId = eventoId;
                _repository.Add<RedeSocial>(entity);
                if(await _repository.SaveChangesAsync())
                {
                    entity = await _repositoryRedeSocial.GetRedeSocialEventoByIdsAsync(eventoId, entity.Id);
                    return _mapper.Map<RedeSocialDto>(entity);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> AddByPalestrante(int palestranteId, RedeSocialDto redeSociaisDto)
        {
            try
            {
                RedeSocial entity = _mapper.Map<RedeSocial>(redeSociaisDto);
                entity.Id = 0;
                entity.PalestranteId = palestranteId;
                entity.EventoId = null;
                _repository.Add<RedeSocial>(entity);
                if(await _repository.SaveChangesAsync())
                {
                    entity = await _repositoryRedeSocial.GetRedeSocialPalestranteByIdsAsync(palestranteId, entity.Id);
                    return _mapper.Map<RedeSocialDto>(entity);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                RedeSocial entity = await _repositoryRedeSocial
                .GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                _repository.Delete<RedeSocial>(entity);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                RedeSocial entity = await _repositoryRedeSocial
                .GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                _repository.Delete<RedeSocial>(entity);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                RedeSocial[] entities = await _repositoryRedeSocial.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialDto[]>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                RedeSocial[] entities = await _repositoryRedeSocial
                .GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialDto[]>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                RedeSocial entity = await _repositoryRedeSocial
                .GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                return _mapper.Map<RedeSocialDto>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                RedeSocial entity = await _repositoryRedeSocial
                .GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                return _mapper.Map<RedeSocialDto>(entity);
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] redesSociaisDto)
        {
            try
            {
                RedeSocial[] entities = await _repositoryRedeSocial.GetAllByEventoIdAsync(eventoId);
    
                foreach(RedeSocialDto redeSocialDto in redesSociaisDto)
                {
                    RedeSocial entity = entities.Where(rs => rs.Id.Equals(redeSocialDto.Id)).FirstOrDefault();
                    if(entity == null)
                    {
                        await AddByEvento(eventoId, redeSocialDto);
                    }
                    else
                    {
                        await UpdateByEvento(eventoId, entity.Id, redeSocialDto);
                    }
                }
                entities = await _repositoryRedeSocial.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialDto[]>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] redesSociaisDto)
        {
            try
            {
                RedeSocial[] entities = await _repositoryRedeSocial.GetAllByPalestranteIdAsync(palestranteId);
    
                foreach(RedeSocialDto redeSocialDto in redesSociaisDto)
                {
                    RedeSocial entity = entities.Where(rs => rs.Id.Equals(redeSocialDto.Id)).FirstOrDefault();
                    if(entity == null)
                    {
                        await AddByPalestrante(palestranteId, redeSocialDto);
                    }
                    else
                    {
                        await UpdateByPalestrante(palestranteId, entity.Id, redeSocialDto);
                    }
                }
                entities = await _repositoryRedeSocial.GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialDto[]>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> UpdateByEvento(int eventoId, int redeSocialId, RedeSocialDto redeSociaisDto)
        {
            try
            {
                RedeSocial entity = _mapper.Map<RedeSocial>(redeSociaisDto);
                entity.Id = redeSocialId;
                entity.EventoId = eventoId;
                _repository.Update<RedeSocial>(entity);
                if(await _repository.SaveChangesAsync()){
                    entity = await _repositoryRedeSocial.GetRedeSocialEventoByIdsAsync(eventoId, entity.Id);
                    return _mapper.Map<RedeSocialDto>(entity);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> UpdateByPalestrante(int palestranteId, int redeSocialId, RedeSocialDto redeSociaisDto)
        {
            try
            {
                RedeSocial entity = _mapper.Map<RedeSocial>(redeSociaisDto);
                entity.Id = redeSocialId;
                entity.PalestranteId = palestranteId;
                _repository.Update<RedeSocial>(entity);
                if(await _repository.SaveChangesAsync()){
                    entity = await _repositoryRedeSocial.GetRedeSocialPalestranteByIdsAsync(palestranteId, entity.Id);
                    return _mapper.Map<RedeSocialDto>(entity);
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