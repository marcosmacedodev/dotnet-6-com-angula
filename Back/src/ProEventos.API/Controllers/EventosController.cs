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
            EventoDto evento = await _service.GetEventoDtoByIdAsync(userId, eventoId);
            if (evento == null) return NotFound();
            evento.ImagemUrl = $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/images/{evento.ImagemUrl}";
            return Ok(evento);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvento(EventoDto evento){
            int userId = User.GetUserId();
            evento = await _service.AddEvento(userId, evento);
            if (evento == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new {eventoId = evento.Id}, evento);
        }

        [HttpPost("{eventoId}/upload-image")]
        public async Task<IActionResult> UploadImage(int eventoId, IFormFile file){
            int userId = User.GetUserId();
            Evento evento = await _service.GetEventoByIdAsync(userId, eventoId);
            if (evento == null) return NotFound();

            if (file.Length > 0){
                 _utils.DeleteImage(evento.ImagemUrl);
                 string path = await _utils.SaveImage(file);
                 //string fullPath =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/images/{path}";
                 evento.ImagemUrl = path;
                 await _service.UpdateEvento(userId, eventoId, evento);
                 return Created(path, NoContent());
            }
            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> UpdateEvento(int eventoId, EventoDto eventoDto){
            int userId = User.GetUserId();
            Evento entity = await _service.GetEventoByIdAsync(userId, eventoId);
            if (entity == null) return NotFound();
            ((EventoService)_service).ValidateEvento(ref entity, eventoDto);
            entity = await _service.UpdateEvento(userId, eventoId, entity);
            
            return NoContent();
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> DeleteEvento(int eventoId){
            int userId = User.GetUserId();
            Evento evento = await _service.GetEventoByIdAsync(userId, eventoId);
            if (evento == null) return NotFound();
            if (await _service.DeleteEvento(evento)) return NoContent();
            return BadRequest();
        }

        [HttpGet("{eventoId}/lotes")]
        public async Task<IActionResult> GetLotesByEventoId(int eventoId){
            LoteDto [] lotes = await _loteService.GetLotesDtoByEventoIdAsync(eventoId);
            if (lotes == null || lotes.Length <= 0) return NoContent();
            return Ok(lotes);
        }

        [HttpGet("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> GetLoteById(int eventoId, int loteId){
            LoteDto lote = await _loteService.GetLoteDtoByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();
            return Ok(lote);
        }

        [HttpPost("{eventoId}/lotes")]
        public async Task<IActionResult> CreateLote(int eventoId, LoteDto entity){
            await _loteService.AddLote(eventoId, entity);
            return NoContent();
        }

        [HttpPut("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> UpdateLote(int eventoId, int loteId, LoteDto entity){
            LoteDto lote = await _loteService.GetLoteDtoByIdsAsync(eventoId, loteId);
            if(lote == null) return NotFound();
            await _loteService.UpdateLote(eventoId, loteId, entity);
            return NoContent();
        }

        [HttpDelete("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId){
            if (await _loteService.DeleteLote(eventoId, loteId))
                return NoContent();
            return BadRequest();
        }
    }
}