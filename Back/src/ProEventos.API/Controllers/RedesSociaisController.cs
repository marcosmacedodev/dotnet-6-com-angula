using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController: ControllerBase
    {
        private readonly IRedeSocialService _service;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedesSociaisController(IRedeSocialService service, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
            _eventoService = eventoService;
            _service = service;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _eventoService.GetEventoByIdAsync(userId, eventoId);
            if(eventoDto == null) Unauthorized();
            RedeSocialDto[] redesSociaisDtos = await _service.GetAllByEventoIdAsync(eventoId);
            if(redesSociaisDtos == null || redesSociaisDtos.Length == 0) return NoContent();
            return Ok(redesSociaisDtos);
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante(){
            int userId = User.GetUserId();
            PalestranteDto palestranteDto = await _palestranteService.GetPalestranteByUserIdAsync(userId,false);
            if(palestranteDto == null) return Unauthorized();
            RedeSocialDto[] redesSociaisDto = await _service.GetAllByPalestranteIdAsync(palestranteDto.Id);
            if(redesSociaisDto == null || redesSociaisDto.Length == 0) return NoContent();
            return Ok(redesSociaisDto);
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] redesSociaisDto){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _eventoService.GetEventoByIdAsync(userId, eventoId);
            if(eventoDto == null) Unauthorized();
            redesSociaisDto = await _service.SaveByEvento(eventoId, redesSociaisDto);
            if(redesSociaisDto == null || redesSociaisDto.Length == 0) return NoContent();
            return Ok(redesSociaisDto);
        }

        [HttpPut("Palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] redesSociaisDto){
            int userId = User.GetUserId();
            PalestranteDto palestranteDto = await _palestranteService.GetPalestranteByUserIdAsync(userId, false);
            if(palestranteDto == null) Unauthorized();
            redesSociaisDto = await _service.SaveByPalestrante(palestranteDto.Id, redesSociaisDto);
            if(redesSociaisDto == null || redesSociaisDto.Length == 0) return NoContent();
            return Ok(redesSociaisDto);
        }

        [HttpDelete("{redeSocialId}/evento/{eventoId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _eventoService.GetEventoByIdAsync(userId, eventoId);
            if(eventoDto == null) Unauthorized();
            bool result = await _service.DeleteByEvento(eventoId, redeSocialId);
            if(!result) return BadRequest($"Não foi possível remover rede social com id: {redeSocialId}");
            return NoContent();
        }
        
        [HttpDelete("{redeSocialId}/palestrante")]
        public async Task<IActionResult> DeleteByPalestrante(int eventoId, int redeSocialId){
            int userId = User.GetUserId();
            PalestranteDto palestranteDto = await _palestranteService.GetPalestranteByUserIdAsync(userId, false);
            if(palestranteDto == null) Unauthorized();
            bool result = await _service.DeleteByPalestrante(palestranteDto.Id, redeSocialId);
            if(!result) return BadRequest($"Não foi possível remover rede social com id: {redeSocialId}");
            return NoContent();
        }
    }
}