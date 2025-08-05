using Data.Implements.BaseData;
using Data.Interfaz;
using Entity.Context;
using Entity.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implement.GameData
{
    public class UserData : BaseData<User>, IUserData
    {
        public UserData(ApplicationDbContext context) : base(context)
        {
        }
    }
}
