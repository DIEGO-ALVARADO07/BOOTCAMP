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
    public class UserBusiness : BaseBusiness<User, UserDTO>, IUserBusiness
    {
        private readonly IUserData _data;


        public UserBusiness(IUserData data, IMapper mapper, ILogger<UserBusiness> logger) 
            : base(data, mapper, logger)
        {
            _data = data;
        }

    }
}
