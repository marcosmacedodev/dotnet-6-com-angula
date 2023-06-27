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
        public EventosController(IEventoService service){
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            return Ok(await _service.GetAllEventosAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            EventoDto evento = await _service.GetEventoByIdAsync(id);
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
        public async Task<IActionResult> Create([FromBody] EventoDto evento){
            evento = await _service.AddEvento(evento);
            if (evento == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new {id = evento.Id}, evento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EventoDto evento){
            evento = await _service.UpdateEvento(id, evento);
            if (evento == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            if (await _service.DeleteEventoById(id)) return NoContent();
            return NotFound();
        }
        
    }
}