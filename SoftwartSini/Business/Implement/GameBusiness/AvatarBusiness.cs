using AutoMapper;
using Business.Interfaz;
using Data.Interfaz;
using Entity.DTO.BaseDTO;
using Entity.Model.Game;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implement.GameBusiness
{
    public class AvatarBusiness : BaseBusiness<Avatar, AvatarDTO>, IAvatarBusiness
    {
        private readonly IAvatarData _data;


        public AvatarBusiness(IAvatarData data, IMapper mapper, ILogger<AvatarBusiness> logger) 
            : base(data, mapper, logger)
        {
            _data = data;
        }

    }
}
