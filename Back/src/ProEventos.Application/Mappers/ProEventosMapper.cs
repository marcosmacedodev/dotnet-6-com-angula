using AutoMapper;
using ProEventos.Domain;
using ProEventos.Domain.Dtos;
using ProEventos.Domain.Identity;

namespace ProEventos.Application.Mappers
{
    public class ProEventosMapper: Profile
    {
        public ProEventosMapper()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
    
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

            CreateMap<Lote, LoteDto>().ReverseMap();

            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();

            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
        }
    }
}