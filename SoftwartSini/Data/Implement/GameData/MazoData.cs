using Data.Implements.BaseData;
using Data.Interfaz;
using Entity.Context;
using Entity.Model.Game;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implement.GameData
{
    public class MazoData : BaseData<Mazo>, IMazoData
    {
        public async Task<Mazo?> GetMazoWithCardsByUserId(int gameId, int userId)
        {
            return await _context.Set<Mazo>()
                .Where(m => m.IdDeparture == gameId && m.Departure.Users == userId)
                .Include(m => m.User)
                .Include(m => m.Cards)
                .FirstOrDefaultAsync();
        }


    }
}
}
