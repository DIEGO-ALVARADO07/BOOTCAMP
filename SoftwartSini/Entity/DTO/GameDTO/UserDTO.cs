using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO.BaseDTO
{
    public class UserDTO : BaseDTO 
    {
        public string user { get; set; }
        public int IdDeparture { get; set; }
        public int IdAvatar { get; set; }
    }
}
