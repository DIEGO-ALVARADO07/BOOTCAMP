using Entity.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaz
{
    public interface IMazoData : IBaseData<Mazo>
    {
        Task<Mazo> GetMazoByUserId(int userId);
        Task<Mazo> GetMazoWithCardsByUserId(int userIdTas, int gameId);
);
    }
}
