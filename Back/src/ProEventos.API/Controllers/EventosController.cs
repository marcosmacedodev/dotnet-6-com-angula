using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Application.Contracts;

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
            return Ok(await _service.GetAllEventosAsync(true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            Evento evento = await _service.GetEventoByIdAsync(id, false);
            if (evento == null) return NotFound();
            return Ok(evento);
        }
        
        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetsByTema(string tema){
            Evento [] eventos = await _service.GetAllEventosByTemaAsync(tema, true);
            if (eventos == null) return NotFound();
            return Ok(eventos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Evento evento){
            evento = await _service.AddEvento(evento);
            if (evento == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new {id = evento.Id}, evento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Evento evento){
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