using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _service;
        public LotesController(ILoteService service){
            _service = service;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetLotesByEventoId(int eventoId){
            LoteDto[] lotes = await _service.GetLotesDtoByEventoIdAsync(eventoId);
            if(lotes == null) return NoContent();
            return Ok(lotes);
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto [] entities){
            LoteDto[] lotes = await _service.SaveLotes(eventoId, entities);
            if(lotes == null) return NoContent();
            return Ok(lotes);
        }
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId){
            LoteDto lote = await _service.GetLoteDtoByIdsAsync(eventoId, loteId);
            if (lote == null) return NotFound();
            if (await _service.DeleteLote(eventoId, loteId)) return NoContent();
            return BadRequest();
        }
    }
}