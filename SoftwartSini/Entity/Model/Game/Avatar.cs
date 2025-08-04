using Entity.Model.Base;
using System.Collections.Generic;
using System.Linq;

namespace Entity.Model.Game
{
    public class Avatar : BaseModel
    {
        public string img {  get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }

    }
}

