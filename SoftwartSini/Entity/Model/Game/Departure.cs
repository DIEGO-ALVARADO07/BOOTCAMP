using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model.Game
{
    public class Departure : BaseModel
    {
        public int quantityUser {  get; set; }
        public int quantityRound { get; set; }
        public int status { get; set; }
        public TimeOnly time { get ; set; }
        public int IdModeGame { get; set; }
        public ModeGame ModeGame { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Mazo> Mazos { get; set; }
        public ICollection<Round> Rounds { get; set; }
        public ICollection<ImageEmoji> ImageEmogis { get; set; }
    }
}
