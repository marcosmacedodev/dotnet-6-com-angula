using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _service;
        //private readonly ILoteService _loteService;
        public EventosController(IEventoService service/*, ILoteService loteService*/){
            _service = service;
            //_loteService = loteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            return Ok(await _service.GetAllEventosAsync());
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId){
            EventoDto evento = await _service.GetEventoByIdAsync(eventoId);
            if (evento == null) return NotFound();
            return Ok(evento);
        }
        
        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetsByTema(string tema){
            EventoDto [] eventos = await _service.GetAllEventosByTemaAsync(tema);
            if (eventos == null) return NotFound();
            return Ok(eventos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvento(EventoDto evento){
            evento = await _service.AddEvento(evento);
            if (evento == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new {eventoId = evento.Id}, evento);
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> UpdateEvento(int eventoId, EventoDto evento){
            evento = await _service.UpdateEvento(eventoId, evento);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> DeleteEvento(int eventoId){
            if (await _service.DeleteEventoById(eventoId)) return NoContent();
            return NotFound();
        }

        // [HttpGet("{eventoId}/lotes")]
        // public async Task<IActionResult> GetLotesByEventoId(int eventoId){
        //     LoteDto [] lotes = await _loteService.GetLotesDtoByEventoIdAsync(eventoId);
        //     if (lotes == null || lotes.Length <= 0) return NoContent();
        //     return Ok(lotes);
        // }

        // [HttpGet("{eventoId}/lotes/{loteId}")]
        // public async Task<IActionResult> GetLoteById(int eventoId, int loteId){
        //     LoteDto lote = await _loteService.GetLoteDtoByIdsAsync(eventoId, loteId);
        //     if (lote == null) return NoContent();
        //     return Ok(lote);
        // }

        // [HttpPost("{eventoId}/lotes")]
        // public async Task<IActionResult> CreateLote(int eventoId, LoteDto entity){
        //     await _loteService.AddLote(eventoId, entity);
        //     return NoContent();
        // }

        // [HttpPut("{eventoId}/lotes/{loteId}")]
        // public async Task<IActionResult> UpdateLote(int eventoId, int loteId, LoteDto entity){
        //     LoteDto lote = await _loteService.GetLoteDtoByIdsAsync(eventoId, loteId);
        //     if(lote == null) return NotFound();
        //     await _loteService.UpdateLote(eventoId, loteId, entity);
        //     return NoContent();
        // }

        // [HttpDelete("{eventoId}/lotes/{loteId}")]
        // public async Task<IActionResult> DeleteLote(int eventoId, int loteId){
        //     if (await _loteService.DeleteLote(eventoId, loteId))
        //         return NoContent();
        //     return BadRequest();
        // }
    }
}