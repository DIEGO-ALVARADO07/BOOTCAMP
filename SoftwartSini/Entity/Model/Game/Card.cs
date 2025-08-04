using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model.Game
{
    public class Card : BaseModel
    {
        public string nameCard { get; set; }
        public int power { get; set; }
        public int force { get; set; }
        public int endurence { get; set; }
        public int speed { get; set; }
        public int technique { get; set; }
        public int intelligence { get; set; }
        public string img {  get; set; }
        public int IdMazo { get; set; }
        public Mazo Mazo { get; set; }
        
    }
}
