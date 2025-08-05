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
    public class CardData : BaseData<Card>, ICardData
    {
        public CardData(ApplicationDbContext context) : base(context)
        {
        }
    }
}
