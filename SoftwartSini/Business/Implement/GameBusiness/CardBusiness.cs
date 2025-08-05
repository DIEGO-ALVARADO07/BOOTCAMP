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
    public class CardBusiness : BaseBusiness<Card, CardDTO>, ICardBusiness
    {
        private readonly ICardData _data;


        public CardBusiness(ICardData data, IMapper mapper, ILogger<CardBusiness> logger) 
            : base(data, mapper, logger)
        {
            _data = data;
        }

    }
}
