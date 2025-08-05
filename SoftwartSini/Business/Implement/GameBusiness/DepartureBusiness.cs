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
    public class DepartureBusiness : BaseBusiness<Departure, DepartureDTO>, IDepartureBusiness
    {
        private readonly IDepartureData _data;


        public DepartureBusiness(IDepartureData data, IMapper mapper, ILogger<DepartureBusiness> logger) 
            : base(data, mapper, logger)
        {
            _data = data;
        }

    }
}
