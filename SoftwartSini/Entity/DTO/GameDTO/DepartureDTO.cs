using Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO.BaseDTO
{
    public class DepartureDTO : BaseDTO
    {
        public int quantityUser {  get; set; }
        public int quantityRound { get; set; }
        public int status { get; set; }
        public ModeGame ModeGame { get; set; }
    }
}
