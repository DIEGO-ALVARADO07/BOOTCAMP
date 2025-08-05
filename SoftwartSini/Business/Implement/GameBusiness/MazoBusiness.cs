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
    public class MazoBusiness : BaseBusiness<Mazo, MazoDTO>, IMazoBusiness
    {
        private readonly IMazoData _data;


        public MazoBusiness(IMazoData data, IMapper mapper, ILogger<MazoBusiness> logger) 
            : base(data, mapper, logger)
        {
            _data = data;
        }

    }
}
