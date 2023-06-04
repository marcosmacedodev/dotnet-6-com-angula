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
    public class EventoController : ControllerBase
    {
    
        [HttpGet]
        public IActionResult GetAll(){
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Evento evento){

            return CreatedAtAction(nameof(Get), new {id = evento.EventoId}, evento);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Evento evento){
            return Ok();
        }
        
    }
}