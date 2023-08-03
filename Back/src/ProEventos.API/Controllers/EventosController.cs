using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.API.Utils;
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
        private readonly IUtilService _utils;
        private readonly IRedeSocialService _redeSocialService;
        public EventosController(IEventoService service, ILoteService loteService, IRedeSocialService redeSocialService, IUtilService utils){
            _redeSocialService = redeSocialService;
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
            if (eventoDto == null) return BadRequest($"Não foi possível criar um novo evento");
            string path =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/{eventoDto.Id}";
            return Created(path, eventoDto);
        }

        [HttpPost("{eventoId}/upload-image")]
        public async Task<IActionResult> UploadImage(int eventoId, IFormFile file){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            if (file.Length > 0)
            {
                 _utils.DeleteImage(eventoDto.ImagemUrl, "images");
                 string path = await _utils.SaveImage(file, "images");
                 string fullPath =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/resources/images/{path}";
                 eventoDto.ImagemUrl = path;
                 await _service.UpdateEvento(userId, eventoId, eventoDto);
                 return Created(fullPath, NoContent());
            }
            return BadRequest($"Erro ao tentar fazer upload de imagem para evento Id: {eventoId}");
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> UpdateEvento(int eventoId, EventoDto eventoDto){
            int userId = User.GetUserId();
            EventoDto entity = await _service.GetEventoByIdAsync(userId, eventoId);
            if (entity == null) return NotFound();
            eventoDto = await _service.UpdateEvento(userId, eventoId, eventoDto);
            return Ok(eventoDto);
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> DeleteEvento(int eventoId){
            int userId = User.GetUserId();
            EventoDto evento = await _service.GetEventoByIdAsync(userId, eventoId);
            if (evento == null) return NotFound();
            bool result = await _service.DeleteEvento(evento);
            if(!result) return BadRequest($"Não foi possível excluir lote com id: {eventoId}");
            return NoContent();
        }

        [HttpGet("{eventoId}/lotes")]
        public async Task<IActionResult> GetLotesByEventoId(int eventoId){
            LoteDto [] lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null || lotes.Length == 0) return NoContent();
            return Ok(lotes);
        }

        [HttpGet("{eventoId}/lotes/{loteId}")]
        public async Task<IActionResult> GetLoteById(int eventoId, int loteId){
            LoteDto lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();
            return Ok(lote);
        }

        [HttpPost("{eventoId}/lotes")]
        public async Task<IActionResult> CreateLote(int eventoId, LoteDto loteDto){
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if(eventoDto == null) return NotFound();
            await _loteService.AddLote(eventoId, loteDto);
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
            if(!result) return BadRequest($"Não foi possível excluir lote com id: {loteId}");  
            return NoContent();
        }

        [HttpPost("{eventoId}/redessociais")]
        public async Task<IActionResult> AddRedeSocial(int eventoId, RedeSocialDto redeSocialDto)
        {
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            redeSocialDto = await _redeSocialService.AddByEvento(eventoId, redeSocialDto);
            if(redeSocialDto == null) return BadRequest($"Não foi possível criar rede social para evento com id: {eventoId}");
            string path =  $"{(Request.IsHttps? "https": "http")}://{Request.Host.Value}/{eventoDto.Id}/redessociais/{redeSocialDto.Id}";
            return Created(path, redeSocialDto);
        }

        [HttpDelete("{eventoId}/redessociais/{redeSocialId}")]
        public async Task<IActionResult> DeleteRedeSocial(int eventoId, int redeSocialId)
        {
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            bool result = await _redeSocialService.DeleteByEvento(eventoId, redeSocialId);
            if(!result) return BadRequest($"Não foi possível excluir rede social com id: {redeSocialId}");
            return NoContent();
        }

        [HttpPut("{eventoId}/redessociais/{redeSocialId}")]
        public async Task<IActionResult> UpdateRedeSocial(int eventoId, int redeSocialId, RedeSocialDto redeSocialDto)
        {
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            redeSocialDto = await _redeSocialService.UpdateByEvento(eventoId, redeSocialId, redeSocialDto);
            if(redeSocialDto == null) return BadRequest($"Não foi possível atualizar rede social com id: {redeSocialId}");
            return NoContent();
        }

        [HttpGet("{eventoId}/redessociais/{redeSocialId}")]
        public async Task<IActionResult> GetRedeSocial(int eventoId, int redeSocialId)
        {
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            RedeSocialDto redeSocialDto = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if(redeSocialDto == null) return NotFound($"Não existe rede social com id: {redeSocialId}");
            return Ok(redeSocialDto);
        }

        [HttpGet("{eventoId}/redessociais")]
        public async Task<IActionResult> GetRedesSocial(int eventoId)
        {
            int userId = User.GetUserId();
            EventoDto eventoDto = await _service.GetEventoByIdAsync(userId, eventoId);
            if (eventoDto == null) return NotFound();
            RedeSocialDto[] redeSocialDtos = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
            if(redeSocialDtos == null || redeSocialDtos.Length == 0) return NoContent();
            return Ok(redeSocialDtos);
        }
        
    }
}