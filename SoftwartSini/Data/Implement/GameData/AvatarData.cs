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
    public class AvatarData : BaseData<Avatar>, IAvatarData
    {
        public AvatarData(ApplicationDbContext context) : base(context)
        {
        }
    }
}
