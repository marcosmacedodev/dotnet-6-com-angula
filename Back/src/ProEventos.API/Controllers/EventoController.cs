using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController(){

        }

        [HttpGet]
        public IEnumerable<String> Get(){
            return new String[] {
                "Valor"
            };
        }
    }
}