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
    public class RoundBusiness : BaseBusiness<Round, RoundDTO>, IRoundBusiness    
    {
        private readonly IRoundData _data;


        public RoundBusiness(IRoundData data, IMapper mapper, ILogger<RoundBusiness> logger) 
            : base(data, mapper, logger)
        {
            _data = data;
        }

    }
}
