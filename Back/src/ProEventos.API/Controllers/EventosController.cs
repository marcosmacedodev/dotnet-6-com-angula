using Microsoft.AspNetCore.Mvc;
using ProEventos.Domain;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.API.Utils;
using ProEventos.Application.Services;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Pages;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _service;
        private readonly ILoteService _loteService;
        private readonly UtilsServices _utils;
        public EventosController(IEventoService service, ILoteService loteService, UtilsServices utils){
            _utils = utils;
            _service = service;
            _loteService = loteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams){
            int userId = User.GetUserId();
            return Ok(await _service.GetAllEventosAsync(userId, pageParams));
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            eventoDto.ImagemUrl = $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/images/{eventoDto.ImagemUrl}";
            return Ok(eventoDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvento(EventoDto eventoDto){
            int userId = User.GetUserId();
            eventoDto = await _service.AddEvento(userId, eventoDto);
            if (eventoDto == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new {eventoId = eventoDto.Id}, eventoDto);
        }

        [HttpPost("{eventoId}/upload-image")]
        public async Task<IActionResult> UploadImage(int eventoId, IFormFile file){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            if (file.Length > 0)
            {
                 _utils.DeleteImage(eventoDto.ImagemUrl);
                 string path = await _utils.SaveImage(file);
                 string fullPath =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/images/{path}";
                 eventoDto.ImagemUrl = path;
                 await _service.UpdateEvento(userId, eventoId, eventoDto);
                 return Created(fullPath, NoContent());
            }
            return BadRequest($"Erro ao tentar fazer upload de imagem para o Evento Id: {eventoId}");
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> UpdateEvento(int eventoId, EventoDto eventoDto){
            int userId = User.GetUserId();
            EventoDto entity = await _service.GetEventoByIdAsync(userId, eventoId);
            if (entity == null) return NotFound();
            eventoDto.Id = eventoId;
            await _service.UpdateEvento(userId, eventoId, eventoDto);
            return NoContent();
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> DeleteEvento(int eventoId){
            int userId = User.GetUserId();
            EventoDto evento = await _service.GetEventoByIdAsync(userId, eventoId);
            if (evento == null) return NotFound();
            if (await _service.DeleteEvento(evento)) return NoContent();
            return BadRequest();
        }

        [HttpGet("{eventoId}/lotes")]
        public async Task<IActionResult> GetLotesByEventoId(int eventoId){
            LoteDto [] lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null || lotes.Length <= 0) return NoContent();
            return Ok(lotes);
        }

        [HttpGet("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> GetLoteById(int eventoId, int loteId){
            LoteDto lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();
            return Ok(lote);
        }

        [HttpPost("{eventoId}/lotes")]
        public async Task<IActionResult> CreateLote(int eventoId, LoteDto entity){
            await _loteService.AddLote(eventoId, entity);
            return NoContent();
        }

        [HttpPut("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> UpdateLote(int eventoId, int loteId, LoteDto updateLoteDto){
            LoteDto loteDto = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if(loteDto == null) return NotFound();
            await _loteService.UpdateLote(eventoId, loteId, updateLoteDto);
            return NoContent();
        }

        [HttpDelete("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId){
            LoteDto loteDto = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if(loteDto == null) return NotFound();
            bool result = await _loteService.DeleteLote(loteDto);
            if(!result) return BadRequest($"Erro ao excluir Evento Id: {eventoId}");  
            return NoContent();
        }
    }
}