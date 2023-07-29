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
            CreateMap<Evento, EventoDto>();
            CreateMap<EventoDto, Evento>();

            CreateMap<User, UserDto>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserUpdateDto, User>();

            CreateMap<Lote, LoteDto>();
            CreateMap<LoteDto, Lote>();
            CreateMap<Lote[], LoteDto[]>();
        }
        
    }
}