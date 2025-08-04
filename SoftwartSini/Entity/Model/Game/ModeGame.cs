using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model.Game
{
    public class ModeGame : BaseModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public Departure Departure { get; set; }
    }
}
