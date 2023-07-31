using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
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
            LoteDto[] lotes = await _service.GetLotesByEventoIdAsync(eventoId);
            if(lotes == null || lotes.Length == 0) return NoContent();
            return Ok(lotes);
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto [] entities){
            LoteDto[] lotes = await _service.SaveLotes(eventoId, entities);
            if(lotes == null || lotes.Length == 0) return NoContent();
            return Ok(lotes);
        }
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId){
            LoteDto loteDto = await _service.GetLoteByIdsAsync(eventoId, loteId);
            if (loteDto == null) return NotFound();
            bool result = await _service.DeleteLote(loteDto);
            if(!result) return BadRequest($"Erro ao remover Lote ID {loteId}");
            return NoContent();
        }
    }
}