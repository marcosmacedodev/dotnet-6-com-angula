using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Dtos;
using ProEventos.Persistence.Pages;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController: ControllerBase
    {
        private readonly IPalestranteService _palestranteService;
        public PalestrantesController(IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
        {
            PageList<PalestranteDto> pageList = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
            return Ok(pageList);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = User.GetUserId();
            PalestranteDto palestranteDto = await _palestranteService.GetPalestranteByUserIdAsync(userId, true);
            if(palestranteDto == null) return NotFound();
            return Ok(palestranteDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PalestranteAddDto palAddDto)
        {
            int userId = User.GetUserId();
            PalestranteDto palestranteDto = await _palestranteService
            .GetPalestranteByUserIdAsync(userId, false);
            if(palestranteDto == null)
            {
               palestranteDto = await _palestranteService.AddPalestrante(userId, palAddDto);
               return Created("", palestranteDto);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(PalestranteUpdateDto palUpDto)
        {
            int userId = User.GetUserId();
            PalestranteDto palestranteDto = await _palestranteService
            .GetPalestranteByUserIdAsync(userId, false);
            if(palestranteDto == null)  return NoContent();
            palestranteDto = await _palestranteService.UpdatePalestrante(userId, palestranteDto.Id, palUpDto);
            return Ok(palestranteDto);
        }
    }
}