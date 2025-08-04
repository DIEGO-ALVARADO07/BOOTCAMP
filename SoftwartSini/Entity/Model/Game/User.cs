using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Base;

namespace Entity.Model.Game
{
    public class User : BaseModel 
    {
        public string user { get; set; }
        public int IdDeparture { get; set; }
        public Departure Departure { get; set; }
        public ICollection<Avatar> Avatar { get; set; } 
    }
}
