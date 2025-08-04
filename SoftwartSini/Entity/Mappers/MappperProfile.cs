using AutoMapper;
using Entity.DTO.BaseDTO;
using Entity.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Avatar, AvatarDTO>().ReverseMap();
            CreateMap<Departure, DepartureDTO>().ReverseMap();
            CreateMap<Round, RoundDTO>().ReverseMap();
            CreateMap<Mazo, MazoDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Card, CardDTO>().ReverseMap();

        }
    }
}
