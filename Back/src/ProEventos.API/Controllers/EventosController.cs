using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Context;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly DataContext _context;
        public EventosController(DataContext context){
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(){
            return Ok(_context.Eventos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){
            Evento? evento = _context.Eventos.Find(id);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Evento evento){
            _context.Add(evento);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new {id = evento.EventoId}, evento);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Evento evento){
            return NoContent();
        }
        
    }
}