using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model.Game
{
    public class ImageEmoji : BaseModel
    {
        public string img {get; set;} 
        public int IdDeparture {get; set;}
        public Departure Departure {get; set;}
    }
}
